using MediatR;
using Whoof.Application.Common.Models;

namespace Whoof.Application.Common.Commands;

public abstract class BaseDeleteCommand<TDto> : IRequest<ServiceResult<TDto>>
    where TDto : class
{
    public Guid Id { get; set; }
}