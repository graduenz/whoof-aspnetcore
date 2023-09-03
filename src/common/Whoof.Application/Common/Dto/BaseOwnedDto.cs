using Swashbuckle.AspNetCore.Annotations;

namespace Whoof.Application.Common.Dto;

public abstract class BaseOwnedDto : BaseDto
{
    /// <summary>
    /// The object's owner
    /// </summary>
    /// <example>john@doe.com</example>
    [SwaggerSchema(ReadOnly = true)]
    public string? OwnerId { get; set; }
}