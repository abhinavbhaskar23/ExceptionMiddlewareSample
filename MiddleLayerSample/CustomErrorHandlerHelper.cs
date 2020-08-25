using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExceptionMiddlewareSample
{
  public static class CustomErrorHandler
  {
    public static void UseException(this IApplicationBuilder app, IHostEnvironment env)
    {
      if(env.IsDevelopment())
      {
        app.Use(WriteResponse);
      }
    }

    private static async Task WriteResponse(HttpContext httpContext, Func<Task> next)
    {
      var exDetails = httpContext.Features.Get<IExceptionHandlerFeature>();
      var ex = exDetails?.Error;

      if(ex != null)
      {
        var problem = new ProblemDetails()
        {
          Title = "An error occured: " + ex.Message,
          Status = 1100,
         Detail = ex.Message.ToString(),
         
        };

        var result = JsonConvert.SerializeObject(problem,new Formatting() { });
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.ContentLength = result.Length;
        await httpContext.Response.WriteAsync(result).ConfigureAwait(false);
      }
    }
  }
}
