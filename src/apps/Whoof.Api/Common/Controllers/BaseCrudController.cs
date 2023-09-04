using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Whoof.Api.Common.Models;
using Whoof.Application.Common.Commands;
using Whoof.Application.Common.Dto;
using Whoof.Application.Common.Models;
using Whoof.Application.Common.Queries;

namespace Whoof.Api.Common.Controllers;

[SuppressMessage("SonarLint", "S2436", Justification = "Abstraction tradeoffs")]
public abstract class BaseCrudController
    <TDto, TEntity, TCreateCommand, TUpdateCommand, TDeleteCommand, TGetByIdQuery, TGetListQuery, TSearch>
    : ControllerBase
    where TDto : BaseDto
    where TEntity : class
    where TCreateCommand : BaseCreateCommand<TDto>, new()
    where TUpdateCommand : BaseUpdateCommand<TDto>, new()
    where TDeleteCommand : BaseDeleteCommand<TDto>, new()
    where TGetByIdQuery : BaseGetByIdQuery<TDto>, new()
    where TGetListQuery : BaseGetListQuery<TDto, TEntity>, new()
    where TSearch : BaseSearch<TEntity>
{
    protected IMediator Mediator { get; }
    protected IValidator<TDto> Validator { get; }

    protected BaseCrudController(IMediator mediator, IValidator<TDto> validator)
    {
        Mediator = mediator;
        Validator = validator;
    }

    protected async Task<IActionResult> GetByIdInternalAsync([FromRoute] Guid id)
    {
        var query = new TGetByIdQuery
        {
            Id = id
        };

        var result = await Mediator.Send(query);
        return result.Succeeded
            ? Ok(result.Data)
            : HandleServiceError(result.Error);
    }

    protected async Task<IActionResult> GetPaginatedListInternalAsync([FromQuery] TSearch request)
    {
        if (request.PageSize > 50)
            request.PageSize = 50;

        var query = new TGetListQuery
        {
            Filters = request.GetFilters()?.ToList() ?? new List<Expression<Func<TEntity, bool>>>(),
            SortExpressions = BuildSortExpressions(request),
            PageIndex = request.PageIndex,
            PageSize = request.PageSize
        };

        var result = await Mediator.Send(query);
        return result.Succeeded
            ? Ok(result.Data)
            : HandleServiceError(result.Error);
    }

    protected async Task<IActionResult> PostInternalAsync([FromBody] TDto model)
    {
        var validationResult = await Validator.ValidateAsync(model);
        if (!validationResult.IsValid)
            return HandleValidationErrors(validationResult);

        var command = new TCreateCommand
        {
            Model = model
        };

        var result = await Mediator.Send(command);
        return result.Succeeded
            ? CreatedAtAction("GetById", new { id = model.Id }, result.Data)
            : HandleServiceError(result.Error);
    }

    protected async Task<IActionResult> PutInternalAsync([FromRoute] Guid id, [FromBody] TDto model)
    {
        var validationResult = await Validator.ValidateAsync(model);
        if (!validationResult.IsValid)
            return HandleValidationErrors(validationResult);

        var command = new TUpdateCommand
        {
            Id = id,
            Model = model
        };

        var result = await Mediator.Send(command);
        return result.Succeeded
            ? Ok(result.Data)
            : HandleServiceError(result.Error);
    }

    protected async Task<IActionResult> DeleteInternalAsync([FromRoute] Guid id)
    {
        var command = new TDeleteCommand
        {
            Id = id
        };

        var result = await Mediator.Send(command);
        return result.Succeeded
            ? Ok(result.Data)
            : HandleServiceError(result.Error);
    }

    protected virtual ICollection<SortExpression> BuildSortExpressions(TSearch request) =>
        string.IsNullOrEmpty(request.Sort)
            ? Array.Empty<SortExpression>()
            : new[]
            {
                new SortExpression(request.Sort,
                    request.Order?.Equals("desc", StringComparison.OrdinalIgnoreCase) ?? false)
            };

    protected virtual IActionResult HandleServiceError(ServiceError? error)
    {
        if (error == null)
            throw new ArgumentNullException(nameof(error));

        var model = new ErrorResult
        {
            Code = error.Code,
            Message = error.Message
        };

        if (error.Equals(ServiceError.NotFound))
            return NotFound(model);

        if (error.Equals(ServiceError.ForbiddenError))
            return Unauthorized(model);

        return BadRequest(model);
    }

    protected virtual IActionResult HandleValidationErrors(ValidationResult validationResult)
    {
        var errors = validationResult.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                k => k.Key,
                v => v.Select(e => e.ErrorMessage).ToList());

        return BadRequest(new ValidationErrorsResult
        {
            Code = ServiceError.Validation.Code,
            Message = ServiceError.Validation.Message,
            Errors = errors
        });
    }
}