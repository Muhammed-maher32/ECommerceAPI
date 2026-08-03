using Asp.Versioning;
using ECommerce.API.Models;
using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/[controller]")]
public class ApiControllerBase : ControllerBase
{
    protected ActionResult Problem(Result result)
    {
        var error = result.Error!;

        var statusCode = result.Error!.Type switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = result.Error.Type switch
        {
            ErrorType.NotFound => "Resource Not Found",
            ErrorType.Validation => "Validation Error",
            ErrorType.Conflict => "Conflict",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.Failure => "Internal Server Error",
            _ => "Internal Server Error"
        };

        var problem = new ProblemDetails
        {
            Title = title,
            Detail = error.Message,
            Status = statusCode
        };

        problem.Extensions["code"] = error.Code;
        problem.Extensions["traceId"] = HttpContext.TraceIdentifier;

        return StatusCode(statusCode, problem);
    }

    protected ActionResult<ApiResponse<IReadOnlyList<T>>> FromPagedResult<T>(
        Result<PagedResult<T>> result,
        int pageNumber,
        int pageSize,
        string successMessage)
        => result.IsFailure ?
        Problem(result)
        : Ok(ApiResponse<IReadOnlyList<T>>.Ok(
            result.Value.Items,
            HttpContext.TraceIdentifier,
            successMessage, new PaginationMeta(pageNumber,
            pageSize, result.Value.TotalCount)));
}
