namespace Whoof.Api.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    
    public Guid CreatedBy { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public string? ModifiedBy { get; set; }
    
    public DateTime ModifiedAt { get; set; }
}