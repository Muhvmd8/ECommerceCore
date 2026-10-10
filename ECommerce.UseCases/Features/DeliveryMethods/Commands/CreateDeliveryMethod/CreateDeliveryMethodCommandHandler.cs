using ECommerce.Domain.Entities.OrderModule;
using ECommerce.Domain.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.Domain.Shared;
using ECommerce.UseCases.Features.DeliveryMethods.Dtos;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.CreateDeliveryMethod;

public sealed class CreateDeliveryMethodCommandHandler(
    IRepository<DeliveryMethod> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDeliveryMethodCommand, Result<DeliveryMethodResponse>>
{
    public async Task<Result<DeliveryMethodResponse>> Handle(
        CreateDeliveryMethodCommand request,
        CancellationToken cancellationToken)
    {
        if (await repository.AnyAsync(new DeliveryMethodByNameSpecification(request.Name), cancellationToken))
            return Result<DeliveryMethodResponse>.Failure(DeliveryMethodErrors.NameAlreadyExists);

        var createResult = DeliveryMethod.Create(
            Guid.NewGuid(),
            request.Name,
            request.Price,
            request.EstimatedDeliveryTime,
            request.Description,
            request.IsAvailable,
            request.DisplayOrder);

        if (createResult.IsFailure)
            return Result<DeliveryMethodResponse>.Failure(createResult.Error!);

        var entity = createResult.Value;
        await repository.AddAsync(entity, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<DeliveryMethodResponse>.Success(ToResponse(entity));
    }

    private static DeliveryMethodResponse ToResponse(DeliveryMethod m) =>
        new(m.Id, m.Name, m.Description, m.Price, m.EstimatedDeliveryTime, m.IsAvailable, m.DisplayOrder);
}
