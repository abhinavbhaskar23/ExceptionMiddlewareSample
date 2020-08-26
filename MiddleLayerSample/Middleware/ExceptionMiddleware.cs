using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionMiddlewareSample.Middleware
{
  public class ExceptionMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IMiddlewareExceptionProcesser _middlewareExceptionProcesser;

    public ExceptionMiddleware(RequestDelegate next, IMiddlewareExceptionProcesser middlewareExceptionProcesser)
    {
      _next = next ?? throw new ArgumentNullException(nameof(next));
      _middlewareExceptionProcesser = middlewareExceptionProcesser;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        if (context.Response.HasStarted)
        {
          throw;
        }

        string result;
        context.Response.Clear();

        (context, result) = await _middlewareExceptionProcesser.Process(ex, context).ConfigureAwait(false);
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsync(result).ConfigureAwait(false);

        return;
      }
    }

  }

}
