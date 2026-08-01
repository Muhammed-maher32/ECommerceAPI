using Asp.Versioning;
using ECommerce.API.Models;
using ECommerce.Domain.Common;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ECommerce.API.Controllers;


[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:ApiVersion}/[controller]")]
public class ApiControllerBase : ControllerBase
{
    protected ActionResult<ApiResponse<T>> Success<T>(T data, string? message = null, PaginationMeta? pagination = null)
    {
        return Ok(ApiResponse<T>.Ok(data, HttpContext.TraceIdentifier, message, pagination));
    }

    protected ActionResult<ApiResponse<T>> FromResult<T>(Result<T> result,
        string? message = null, PaginationMeta? pagination = null)
    {
        return result.IsFailure
            ? Problem(result)
            : Success(result.Value, message, pagination);
    }
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
}
