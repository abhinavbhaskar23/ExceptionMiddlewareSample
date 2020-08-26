using ExceptionMiddleware;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExceptionMiddlewareSample
{
 
  public class MiddlewareExceptionHandler : IExceptionHandler
  {
    public async Task<(HttpContext,string)> Process(Exception ex, HttpContext context)
    {
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

      return (context, result);
    }

    private static string BuildResultold(dynamic message, string errorCode = "Unhandled Error")
    {
      return JsonConvert.SerializeObject(
          new
          {
            errors = message,//ex.ValidationMessages
            errorCode = errorCode,
          });
    }

    private static string BuildResult(dynamic message, string errorCode = "Error")
    {
      return JsonConvert.SerializeObject(
          new
          {
            errors = message,//ex.ValidationMessages
            errorCode = errorCode,
            source= "Custom Exception"
          });
    }
  }
}

