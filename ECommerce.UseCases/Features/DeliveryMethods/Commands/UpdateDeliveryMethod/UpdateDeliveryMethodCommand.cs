using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.DeliveryMethods.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.UpdateDeliveryMethod;

public sealed record UpdateDeliveryMethodCommand(
    Guid Id,
    string Name,
    decimal Price,
    string EstimatedDeliveryTime,
    string? Description = null,
    bool IsAvailable = true,
    int DisplayOrder = 0) : IRequest<Result<DeliveryMethodResponse>>;
