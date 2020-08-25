using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Middleware
{
  // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
  public class DataModelExceptionMiddleware
  {
    private readonly RequestDelegate _next;

    public DataModelExceptionMiddleware(RequestDelegate next)
    {
      _next = next ?? throw new ArgumentNullException(nameof(next));
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

        await GetSchemaConverterAsync(ex, context).ConfigureAwait(false);

        return;
      }
    }

    public static Task GetSchemaConverterAsync(Exception ex, HttpContext context)
    {
      context.Response.Clear();
      //HttpResponse response = context.Response;
      string result = "";
      switch (ex.GetType().Name)
      {
        case "ArgumentNullException":
          result = BuildResult(ex.Message);
          context.Response.StatusCode = StatusCodes.Status400BadRequest;
          break;
        case "ValidationException":
          context.Response.StatusCode = StatusCodes.Status422UnprocessableEntity;
          List<string> errors = ex.Message.Split("\r\n").ToList();
          result = BuildResult(errors);
          break;
        default:
          context.Response.StatusCode = StatusCodes.Status500InternalServerError;
          result = BuildResult(ex.Message);
          break;
      }
      
      context.Response.ContentType = "application/json";
      return context.Response.WriteAsync(result);

    }

    private static string BuildResult(dynamic message, string errorCode = "Error")
    {
      return JsonConvert.SerializeObject(
          new
          {
            errors = message,//ex.ValidationMessages
            errorCode = errorCode,
          });
    }
  }


  // Extension method used to add the middleware to the HTTP request pipeline.
  public static class HttpStatusCodeExceptionMiddlewareExtensions
  {
    public static IApplicationBuilder UseHttpStatusCodeExceptionMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<DataModelExceptionMiddleware>();
    }
  }
}
