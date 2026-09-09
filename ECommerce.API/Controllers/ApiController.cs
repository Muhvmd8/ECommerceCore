using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace ECommerce.API.Controllers;
[Route("api/[controller]")]
[ApiController]
internal abstract class ApiController : ControllerBase
{
    [Authorize]
    protected string _GetIdFromToken() => User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

    [Authorize]
    protected string _GetEmailFromToken() => User.FindFirst(ClaimTypes.Name)?.Value!;
}