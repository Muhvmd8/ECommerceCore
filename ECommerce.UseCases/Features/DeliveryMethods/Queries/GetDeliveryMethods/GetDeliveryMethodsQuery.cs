using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.DeliveryMethods.Dtos;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethods;

public sealed record GetDeliveryMethodsQuery(
    bool AvailableOnly = true) : IRequest<Result<IReadOnlyList<DeliveryMethodResponse>>>;
