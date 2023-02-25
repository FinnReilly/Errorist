# Errorist
## A compact and configurable package for formatting API exception output in .NET 6
-----------------

Errorist allows API developers to quickly configure the way that errors thrown in the application are displayed to the API consumer. Developers can alter output based on the type of exception, the controller or service it was thrown from, or specific attributes of the exception itself.  Use the built-in `ApiExceptionDto` or register your own output models.

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