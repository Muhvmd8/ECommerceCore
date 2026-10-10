using ECommerce.API.Attributes;
using ECommerce.API.Controllers.Extensions;
using ECommerce.API.Models;
using ECommerce.UseCases.Features.Baskets.Commands.AddBasketItem;
using ECommerce.UseCases.Features.Baskets.Commands.ClearBasket;
using ECommerce.UseCases.Features.Baskets.Commands.MergeBasket;
using ECommerce.UseCases.Features.Baskets.Commands.RemoveBasketItem;
using ECommerce.UseCases.Features.Baskets.Commands.UpdateBasketItemQuantity;
using ECommerce.UseCases.Features.Baskets.DTOs;
using ECommerce.UseCases.Features.Baskets.Queries.GetBasket;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.API.Controllers
{
    [RequireBuyerId]
    public class BasketsController(IMediator mediator) : ApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> Get(CancellationToken cancellationToken = default)
        {
            var buyerIdResult = HttpContext.GetBuyerId();
            var result = await mediator.Send(new GetBasketQuery(buyerIdResult.Value), cancellationToken);
            return FromResult(result, ApiMessages.BasketRetrieved);
        }

        [HttpPost("items")]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> AddItem(
            AddBasketItemCommand request,
            CancellationToken cancellationToken = default)
        {
            var buyerIdResult = HttpContext.GetBuyerId();
            var command = new AddBasketItemCommand(buyerIdResult.Value, request.ProductId, request.Quantity);
            var result = await mediator.Send(command, cancellationToken);

            return FromResult(result, ApiMessages.BasketItemAdded);
        }

        [HttpPut("items/{productId:int}")]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> UpdateItemQuantity(
            int productId,
            int quantity,
            CancellationToken cancellationToken = default)
        {
            var buyerIdResult = HttpContext.GetBuyerId();
            var command = new UpdateBasketItemQuantityCommand(buyerIdResult.Value, productId, quantity);
            var result = await mediator.Send(command, cancellationToken);

            return FromResult(result, ApiMessages.BasketItemUpdated);
        }

        [HttpDelete("items/{productId:int}")]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> UpdateItemQuantity(
            int productId,
            CancellationToken cancellationToken = default)
        {
            var buyerIdResult = HttpContext.GetBuyerId();
            var command = new RemoveBasketItemCommand(buyerIdResult.Value, productId);
            var result = await mediator.Send(command, cancellationToken);

            return FromResult(result, ApiMessages.BasketItemRemoved);
        }

        [HttpDelete("clear")]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> Clear(

            CancellationToken cancellationToken = default)
        {
            var buyerIdResult = HttpContext.GetBuyerId();
            var result = await mediator.Send(new ClearBasketCommand(buyerIdResult.Value), cancellationToken);
            return FromResult(result, ApiMessages.BasketCleared);
        }

        [HttpPost("merge")]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> Merge(
            Guid anonymousBuyerId,
            CancellationToken cancellationToken = default)
        {
            var buyerIdResult = HttpContext.GetBuyerId();
            var command = new MergeBasketCommand(buyerIdResult.Value, anonymousBuyerId);
            var result = await mediator.Send(command, cancellationToken);

            return FromResult(result, ApiMessages.BasketMerged);
        }
    }
}
