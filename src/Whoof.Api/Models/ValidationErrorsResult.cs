using FluentValidation.Results;

namespace Whoof.Api.Models;

public class ValidationErrorsResult : IBadRequest
{
    public string? Type { get; set; }
    public List<ValidationFailure>? Errors { get; set; }
}