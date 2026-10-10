using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.DeliveryMethods.Dtos;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethods;

public sealed class GetDeliveryMethodsQueryHandler(IReadRepository<DeliveryMethod> repository)
    : IRequestHandler<GetDeliveryMethodsQuery, Result<IReadOnlyList<DeliveryMethodResponse>>>
{
    public async Task<Result<IReadOnlyList<DeliveryMethodResponse>>> Handle(
        GetDeliveryMethodsQuery request,
        CancellationToken cancellationToken)
    {
        var items = await repository.ListAsync(
            new DeliveryMethodsListSpecification(request.AvailableOnly),
            cancellationToken);

        return Result<IReadOnlyList<DeliveryMethodResponse>>.Success(items);
    }
}
