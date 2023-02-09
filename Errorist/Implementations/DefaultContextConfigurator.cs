using Errorist.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Errorist.Implementations
{
    public class DefaultContextConfigurator<TOutput> : IHttpContextConfigurator<TOutput>
        where TOutput : class, new()
    {
        public async Task ConfigureContextWithErrorResponse(HttpContext httpContext, TOutput output)
        {
            if (output is IHasStatusCode statusCodeModel)
            {
                httpContext.Response.StatusCode = statusCodeModel.StatusCode;
            }
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(output));
        }
    }
}
