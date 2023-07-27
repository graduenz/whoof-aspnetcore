namespace Whoof.Domain.Common;

public class BaseOwnedEntity : BaseEntity
{
    public string? OwnerId { get; set; }
}