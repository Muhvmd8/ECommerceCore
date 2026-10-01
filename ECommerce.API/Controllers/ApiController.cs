using ECommerce.API.Models;
using ECommerce.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ECommerce.API.Controllers;
[ApiController]
[Route("api/[controller]")]
// automatic model validation
// detect data from (form, query, route, etc)
// when the problem is detected, it will return a object of problem details by default.
public abstract class ApiController : ControllerBase
{
    [Authorize]
    protected string _GetIdFromToken() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

    [Authorize]
    protected string _GetEmailFromToken() => User.FindFirst(ClaimTypes.Name)?.Value!;

    protected ActionResult<ApiResponse<T>> Success<T>(T data,
        string? message = null, PaginationMeta? pagination = null)
    {
        var response = ApiResponse<T>.Ok(data, HttpContext.TraceIdentifier, message, pagination);
        return Ok(response);
    }

    protected ActionResult Problem(Result result)
    {
        var statusCode = result?.Error?.ErrorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = result?.Error?.ErrorType switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Not Found",
            ErrorType.Unauthorized => "Unauthorized",
            ErrorType.Forbidden => "Forbidden",
            ErrorType.Conflict => "Conflict",
            _ => "Internal Server Error"
        };

        var problem = new Dictionary<string, object?>()
        {
            ["type"] = $"https://httpstatuses.com/{result?.Error?.Code}",
            ["title"] = title,
            ["status"] = statusCode
        };

        if (result?.Error?.ErrorType is ErrorType.Validation)
            problem["errors"] = new Dictionary<string, string[]>
            {
                [result.Error.Code] = [result.Error.Message]
            };
        else
            problem["detail"] = result?.Error?.Message;

        problem["traceId"] = HttpContext.TraceIdentifier;

        return new ObjectResult(problem)
        {
            StatusCode = statusCode
        };
    }

    protected ActionResult<ApiResponse<T>> FromResult<T>(Result<T> result, string? message = null)
    {
        if (result.IsSuccess)
            return Success(result.Value, message);

        return Problem(result);
    }

    protected ActionResult<ApiResponse<IReadOnlyList<T>>> FromPagedResult<T>(
        Result<PagedResult<T>> result,
        int pageNumber,
        int pageSize,
        string successMessage)
    {
        if (result.IsFailure)
            return Problem(result);
        else
            return Success(result.Value.Items, successMessage, new PaginationMeta(pageNumber, pageSize, result.Value.TotalCount));
    }



}