using Microsoft.AspNetCore.Mvc.Filters;

namespace EduResourceAPI.Controllers.ErrorHandler
{
    public class ErrorHandlingFilter : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            ILogger logger = LoggerFactory.Create(x => x.AddConsole()).CreateLogger("ErrorHandlingFilter");
            logger.LogError($"Exception message: {exception.Message}\nStack trace:\n{exception.StackTrace}");

            context.HttpContext.Response.StatusCode = 500;
            context.ExceptionHandled = true; 
        }
    }
}
