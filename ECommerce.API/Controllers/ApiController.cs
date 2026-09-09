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
}