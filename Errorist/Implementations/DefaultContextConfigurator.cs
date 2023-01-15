using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Errorist.Implementations
{
    public class DefaultContextConfigurator<TOutput> : IHttpContextConfigurator<TOutput>
        where TOutput : class, new()
    {
        public Task ConfigureContextWithErrorResponse(HttpContext httpContext, TOutput output)
            => httpContext.Response.WriteAsync(JsonConvert.SerializeObject(output));
    }
}
