using ECommerce.API.Controllers.Extensions;
using ECommerce.API.Factories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.API.Attributes
{
    public class RequireBuyerIdAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var buyerIdResult = context.HttpContext.GetBuyerId();

            if (buyerIdResult.IsFailure)
            {
                context.Result = ApiResponseFactory.Problem(
                    buyerIdResult,
                    context.HttpContext.TraceIdentifier);

                return;
            }

            context.HttpContext.Items["BuyerId"] = buyerIdResult.Value;

            await next();
        }
    }
}
