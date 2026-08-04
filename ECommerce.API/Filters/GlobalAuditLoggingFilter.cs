using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace ECommerce.API.Filters;

public class GlobalAuditLoggingFilter(ILogger<GlobalAuditLoggingFilter> logger) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var userId = context.HttpContext.User.FindFirst("sub")?.Value;
        var actionName = context.ActionDescriptor.DisplayName;

        var executedContext = await next();

        logger.LogInformation("User {UserId} executed {ActionName} With Status Code {StatusCode}",
            userId,
            actionName,
            executedContext.HttpContext.Response.StatusCode);
    }
}
