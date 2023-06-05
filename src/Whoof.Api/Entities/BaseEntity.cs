namespace Whoof.Api.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid CreatedBy { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    
    public string? ModifiedBy { get; set; }
    
    public DateTimeOffset ModifiedAt { get; set; }
}