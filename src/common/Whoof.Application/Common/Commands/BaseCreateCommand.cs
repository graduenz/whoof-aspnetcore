using MediatR;
using Whoof.Application.Common.Models;

namespace Whoof.Application.Common.Commands;

public abstract class BaseCreateCommand<TDto> : IRequest<ServiceResult<TDto>>
    where TDto : class
{
#pragma warning disable CS8618
    public TDto Model { get; set; }
#pragma warning restore CS8618
}