using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace Middlewares
{
  public class ApplicationEvents
  {
    private readonly RequestDelegate _next;

    public ApplicationEvents(RequestDelegate next, IConfiguration config)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      // Do something with context near the beginning of request processing.
      BeginInvoke(context);
      await _next.Invoke(context);
      // if request is unknown, redirect to home page.
      if (context.Response.StatusCode == 404 &&
          !Path.HasExtension(context.Request.Path.Value) &&
          !context.Request.Path.Value.StartsWith("/api"))
      {
        context.Request.Path = "/index.html";
        context.Response.StatusCode = 200; // Make sure we update the status code, otherwise it returns 404
        await _next.Invoke(context);
      }
      EndInvoke(context);
    }

    #region Private Methods

    private void BeginInvoke(HttpContext context)
    {
    
    }

    private void EndInvoke(HttpContext context)
    {
    }
    #endregion

  }
  public static class ApplicationEventsExtensions
  {
    public static IApplicationBuilder UseApplicationEvents(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<ApplicationEvents>();
    }
  }
}
