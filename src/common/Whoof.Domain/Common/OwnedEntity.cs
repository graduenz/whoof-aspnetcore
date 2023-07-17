namespace Whoof.Domain.Common;

public class OwnedEntity : BaseEntity
{
    public string? OwnerId { get; set; }
}