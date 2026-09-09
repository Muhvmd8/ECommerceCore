namespace ECommerce.Domain.Entities.Common;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;

    public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
    public void SetUpdatedAt(DateTime updatedAt) => UpdatedAt = updatedAt;
    public void MarkAsDeleted()
    {
        IsDeleted = true;
        UpdatedAt = DateTime.UtcNow;
    }
}

public abstract class BaseEntity<TKey> : BaseEntity
{
    public TKey Id { get; protected set; } = default!;
}
