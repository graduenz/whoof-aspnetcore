using MediatR;
using Whoof.Application.Common.Models;

namespace Whoof.Application.Common.Commands;

public abstract class BaseUpdateCommand<TDto> : IRequest<ServiceResult<TDto>>
    where TDto : class
{
    public Guid Id { get; set; }
    
#pragma warning disable CS8618
    public TDto Model { get; set; }
#pragma warning restore CS8618
}