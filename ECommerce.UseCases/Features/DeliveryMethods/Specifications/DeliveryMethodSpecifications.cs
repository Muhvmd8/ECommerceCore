using ECommerce.Domain.Entities.OrderModule;
using ECommerce.UseCases.Features.DeliveryMethods.Dtos;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.DeliveryMethods.Specifications;

public sealed class DeliveryMethodsListSpecification : Specification<DeliveryMethod, DeliveryMethodResponse>
{
    public DeliveryMethodsListSpecification(bool availableOnly)
    {
        var query = Query;

        if (availableOnly)
            query.Where(m => m.IsAvailable);

        query
            .OrderBy(m => m.DisplayOrder);

        query.Select(m => new DeliveryMethodResponse(
            m.Id,
            m.Name,
            m.Description,
            m.Price,
            m.EstimatedDeliveryTime,
            m.IsAvailable,
            m.DisplayOrder));
    }
}

public sealed class DeliveryMethodByIdSpecification : Specification<DeliveryMethod, DeliveryMethodResponse>
{
    public DeliveryMethodByIdSpecification(Guid id)
    {
        Query
            .Where(m => m.Id == id)
            .Select(m => new DeliveryMethodResponse(
                m.Id,
                m.Name,
                m.Description,
                m.Price,
                m.EstimatedDeliveryTime,
                m.IsAvailable,
                m.DisplayOrder));
    }
}

public sealed class DeliveryMethodEntityByIdSpecification : Specification<DeliveryMethod>
{
    public DeliveryMethodEntityByIdSpecification(Guid id)
        => Query.Where(m => m.Id == id);
}

public sealed class DeliveryMethodByNameSpecification : Specification<DeliveryMethod>
{
    public DeliveryMethodByNameSpecification(string name, Guid? excludeId = null)
    {
        var normalized = name.Trim();
        Query.Where(m => m.Name == normalized);

        if (excludeId.HasValue)
            Query.Where(m => m.Id != excludeId.Value);
    }
}
