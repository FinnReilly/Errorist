# Errorist

## A compact and configurable package for formatting API exception output in .NET 6
-----------------

Errorist allows API developers to quickly configure the way that errors thrown in the application are displayed to the API consumer. Developers can alter output based on the type of exception, the controller or service it was thrown from, or specific attributes of the exception itself.  

Use the built-in `ApiExceptionDto`, or register your own output models. Use your own builder classes to write clear and easily readable configurations.

## Getting started

To use Errorist in your application, assuming that you wish to use the in-built `ApiExceptionDto` output model, you will first need to add the following code to your `Program` file:

```
builder.Services.AddErrorConfiguration<ApiExceptionDto>();
```

After the `builder.Build()` method has been called, you will need to add the middleware to the application's pipeline:

```
app.UseErrorConfigurationMiddleware<ApiExceptionDto>();
```

## Global configurations

Whilst in the `Program` file, it is recommended that you add some Global configurations - these can then be overridden throughout the application as needed. For this you can use `app.UseGlobalDefaultErrorConfiguration<ApiExceptionDto>` for all exceptions, or `app.UseGlobalErrorConfiguration<ApiExceptionDto,TException>` for specific types.

For example, using the built-in generic builder:
```
app.UseGlobalDefaultErrorConfiguration<ApiExceptionDto>()
    .AddConfiguration((e, dto) => 
    {
        dto.Title = "Something went wrong",
        dto.Message = $"The application threw an exception of type {e.GetType().FullName}",
        dto.UserAdvice = "Please contact your technical team",
        dto.StatusCode = 500
    });
    
app.UseGlobalErrorConfiguration<ApiExceptionDto, AuthenticationException>()
    .AddConfiguration((e, dto) =>
    {
        dto.StatusCode = 401,
        dto.UserAdvice = "Try logging in"
    });
```
If no further configuration is added beyond these global configurations, most users with an error will receive the following json (or similar) with a status code of 500:
```
{
    "title": "Something went wrong",
    "message": "The application threw an exception of type System.Exception",
    "userAdvice": "Please contact your technical team"
}
```
Whereas those experiencing an AuthenticationException will receive the following, with a status code of 401:
```
{
    "title": "Something went wrong",
    "message": "The application threw an exception of type MyApplication.Exceptions.AuthenticationException",
    "userAdvice": "Try logging in"
}
```

## Configuration Scopes
Errorist allows you to override global configurations and configure error output according to the controller or service method that an exception is thrown in.  In order to do so, you will need to inject the `IExceptionScopeProvider<TOutput>` dependency into the class or controller, and create an `IExceptionFormattingScope<TOutput>` as follows:
```
using var exceptionScope = _scopeProvider.GetScope();
```
This scope can then be used to create default or exception-specific error configurations in much the same way that you would create global configurations.  Using an example based on the `ApiExceptionDto` again, and with the global configuration defined above, the following is added in a Get method on a 'Users' controller:
```
exceptionScope.ConfigureDefault()
    .AddConfiguration((exception, dto) =>
    {
        dto.Title = "Error on users controller";
        dto.Message = "Could not get users";
    });
    
exceptionScope.Configure<TaskCanceledException>()
    .AddConfiguration((exception, dto) => 
    {
        dto.Message = "Could not get users due to a timeout"
    })
```
In this case users will receive the following output in the event of most exceptions:
```
{
    "title": "Error on users controller",
    "message": "Could not get users",
    "userAdvice": "Please contact your technical team"
}
```
and the following in the case of a `TaskCanceledException`:
```
{
    "title": "Error on users controller",
    "message": "Could not get users due to a timeout",
    "userAdvice": "Please contact your technical team"
}
```
Any configurations defined using a configuration scope will be applied to exceptions thrown within that scope.

**Important** - `IExceptionScopeProvider<TOutput>` is always scoped to the lifetime of the request.  If you wish to use configuration scopes within a Singleton service, you will do to inject an `IExceptionScopeProviderFactory<TOutput>` and use it as follows:
```
using var exceptionScope = _providerFactory.CurrentProvider.GetScope();
```

## Order of Precedence
Within each scope (including the global scope) configurations are applied in the following order:

1. The default configuration, if it exists
2. The specific configuration for the type of exception thrown, if it exists

The scopes themselves are then applied in the following order:

1. The Global scope configured in `Program`
2. The scope at the lowest level of the call stack (eg. if a configuration scope has been used within a method in your Repository layer)
3. Each scope which is exited as the Exception moves up the call stack
4. Finally, the scope which is configured the highest up the call stack is applied last

Essentially, this means that your error output configurations will roughly follow the principle of 'Most Specific Wins'. 

It is *not* recommended to use nested configuration scopes in the same method, as this will cause the outer scope to override the configurations made in the inner.