namespace Whoof.Api.Common.Models;

public class ValidationErrorsResult : ErrorResult
{
    public Dictionary<string, List<string>>? Errors { get; set; }
}