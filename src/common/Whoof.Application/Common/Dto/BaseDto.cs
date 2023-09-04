using Swashbuckle.AspNetCore.Annotations;

namespace Whoof.Application.Common.Dto;

public abstract class BaseDto
{
    /// <summary>
    /// The object's unique identifier
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// When the object was created
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public DateTimeOffset CreatedAt { get; set; }
    
    /// <summary>
    /// Who created the object
    /// </summary>
    /// <example>john@doe.com</example>
    [SwaggerSchema(ReadOnly = true)]
    public string? CreatedBy { get; set; }
    
    /// <summary>
    /// When the object was last modified
    /// </summary>
    [SwaggerSchema(ReadOnly = true)]
    public DateTimeOffset ModifiedAt { get; set; }
    
    /// <summary>
    /// Who modified the object last time
    /// </summary>
    /// <example>john@doe.com</example>
    [SwaggerSchema(ReadOnly = true)]
    public string? ModifiedBy { get; set; }
}