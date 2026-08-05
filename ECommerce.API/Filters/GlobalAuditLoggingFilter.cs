using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.API.Filters;

public class GlobalAuditLoggingFilter(ILogger<GlobalAuditLoggingFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionName = context.ActionDescriptor.DisplayName;

        var executedContext = await next();

        logger.LogInformation("Executed {ActionName} With Status Code {StatusCode}",
            actionName,
            executedContext.HttpContext.Response.StatusCode);
    }
}
