using FluentValidation.Results;

namespace Whoof.Application.Common.Models;

public class ValidationErrorsResult : IBadRequest
{
    public string? Type { get; set; }
    public List<ValidationFailure>? Errors { get; set; }
}