using Microsoft.AspNetCore.Http;

namespace Errorist
{
    public interface IHttpContextConfigurator<TOutput>
        where TOutput : class, new()
    {
        Task ConfigureContextWithErrorResponse(HttpContext httpContext, TOutput output);
    }
}
