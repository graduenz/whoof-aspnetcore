using MediatR;
using Whoof.Application.Common.Models;

namespace Whoof.Application.Common.Queries;

public abstract class BaseGetByIdQuery<TDto> : IRequest<ServiceResult<TDto>>
    where TDto : class
{
    public Guid Id { get; set; }
}