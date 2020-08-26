using Microsoft.AspNetCore.Builder;

namespace ExceptionMiddleware
{
  // Extension method used to add the middleware to the HTTP request pipeline.
  public static class CustomMiddlewareExtensions
  {
    public static IApplicationBuilder UseCustomExceptionMiddlewareNew(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ExceptionMiddleware>();
    }
  }

}
