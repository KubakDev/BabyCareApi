using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BabyCareApi.Common;

public class UnhandledExceptionFilter : IExceptionFilter
{
  private readonly ILogger<UnhandledExceptionFilter> _Logger;

  public UnhandledExceptionFilter(ILogger<UnhandledExceptionFilter> logger)
  {
    _Logger = logger;
  }

  public void OnException(ExceptionContext context)
  {
    _Logger.LogCritical(context.Exception, "An unhandled error occured.");

    var problem = new ProblemDetails
    {
      Type = context.Exception.GetType().Name,
      Title = "Server Error",
      Detail = context.Exception.Message,
    };

    problem.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

    context.Result = new ObjectResult(problem)
    {
      StatusCode = 500
    };
  }
}