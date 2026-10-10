using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.DeliveryMethods.Dtos;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethodById;

public sealed class GetDeliveryMethodByIdQueryHandler(IReadRepository<DeliveryMethod> repository)
    : IRequestHandler<GetDeliveryMethodByIdQuery, Result<DeliveryMethodResponse>>
{
    public async Task<Result<DeliveryMethodResponse>> Handle(
        GetDeliveryMethodByIdQuery request,
        CancellationToken cancellationToken)
    {
        var item = await repository.FirstOrDefaultAsync(
            new DeliveryMethodByIdSpecification(request.Id),
            cancellationToken);

        if (item is null)
            return Result<DeliveryMethodResponse>.Failure(DeliveryMethodErrors.NotFound);

        return Result<DeliveryMethodResponse>.Success(item);
    }
}
