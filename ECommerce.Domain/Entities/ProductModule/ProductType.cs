using ECommerce.Domain.Entities.Common;

namespace ECommerce.Domain.Entities.ProductModule;

public class ProductType : BaseEntity<int>
{
    public string Name { get; private set; } = default!;

    public ICollection<Product> Products { get; private set; } = [];

    private ProductType(string name)
    {
        SetName(name);
    }

    public static ProductType Create(string name) => new(name);

    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Product brand name cannot be null or empty.",
                nameof(name));

        Name = name.Trim();
    }
}