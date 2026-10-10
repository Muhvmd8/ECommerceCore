using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.DeliveryMethods.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.CreateDeliveryMethod;

public sealed record CreateDeliveryMethodCommand(
    string Name,
    decimal Price,
    string EstimatedDeliveryTime,
    string? Description = null,
    bool IsAvailable = true,
    int DisplayOrder = 0) : IRequest<Result<DeliveryMethodResponse>>;
