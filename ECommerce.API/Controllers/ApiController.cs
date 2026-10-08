using ECommerce.API.Factories;
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

    protected ActionResult<ApiResponse<T>> Success<T>(
        T data,
        string? message = null,
        PaginationMeta? pagination = null)
    {
        var response = ApiResponseFactory.Success(
            data,
            HttpContext.TraceIdentifier,
            message,
            pagination);
        return Ok(response);
    }

    protected ActionResult Problem(Result result)
        => ApiResponseFactory.Problem(
                result,
                HttpContext.TraceIdentifier);

    protected ActionResult<ApiResponse<T>> FromResult<T>(
        Result<T> result,
        string? message = null)
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

        return Success(
            result.Value.Items,
            successMessage,
            new PaginationMeta(
                pageNumber,
                pageSize,
                result.Value.TotalCount));
    }
}
