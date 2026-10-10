using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.DeliveryMethods.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethodById;

public sealed record GetDeliveryMethodByIdQuery(Guid Id)
    : IRequest<Result<DeliveryMethodResponse>>;
