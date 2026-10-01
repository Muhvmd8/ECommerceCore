using ECommerce.API.Controllers.Extensions;
using ECommerce.API.Models;
using ECommerce.UseCases.Baskets.Commands;
using ECommerce.UseCases.Baskets.DTOs;
using ECommerce.UseCases.Baskets.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace ECommerce.API.Controllers
{
    public class BasketsController(IMediator mediator) : ApiController
    {

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> Get(CancellationToken cancellationToken = default)
        {
            var buyerIdResult = this.GetBuyerId();
            if (buyerIdResult.IsFailure)
                return Problem(buyerIdResult);

            var result = await mediator.Send(new GetBasketQuery(buyerIdResult.Value), cancellationToken);
            return FromResult(result, "Basket Retrieved Successfully");
        }

        [HttpPost("items")]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> AddItem(
            AddBasketItemCommand request,
            CancellationToken cancellationToken = default)
        {
            var buyerIdResult = this.GetBuyerId();
            if (buyerIdResult.IsFailure)
                return Problem(buyerIdResult);

            var command = new AddBasketItemCommand(buyerIdResult.Value, request.ProductId, request.Quantity);
            var result = await mediator.Send(command, cancellationToken);

            return FromResult(result, "Item Added To Basket Successfully");
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
            var buyerIdResult = this.GetBuyerId();
            if (buyerIdResult.IsFailure)
                return Problem(buyerIdResult);

            var command = new UpdateBasketItemQuantityCommand(buyerIdResult.Value, productId, quantity);
            var result = await mediator.Send(command, cancellationToken);

            return FromResult(result, "Basket Item Quantity Updated Successfully");
        }

        [HttpDelete("items/{productId:int}")]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> UpdateItemQuantity(
            int productId,
            CancellationToken cancellationToken = default)
        {
            var buyerIdResult = this.GetBuyerId();
            if (buyerIdResult.IsFailure)
                return Problem(buyerIdResult);

            var command = new RemoveBasketItemCommand(buyerIdResult.Value, productId);
            var result = await mediator.Send(command, cancellationToken);

            return FromResult(result, "Basket Item Removed Successfully");
        }

        [HttpPost("merge")]
        [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<GetBasketResponse>>> Merge(
            Guid anonymousBuyerId,
            CancellationToken cancellationToken = default)
        {
            var buyerIdResult = this.GetBuyerId();
            if (buyerIdResult.IsFailure)
                return Problem(buyerIdResult);

            var command = new MergeBasketCommand(buyerIdResult.Value, anonymousBuyerId);
            var result = await mediator.Send(command, cancellationToken);

            return FromResult(result, "Basket Merged Successfully");
        }
    }
}
