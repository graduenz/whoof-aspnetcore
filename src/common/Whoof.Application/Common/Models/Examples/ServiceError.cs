using Swashbuckle.AspNetCore.Filters;

namespace Whoof.Application.Common.Models.Examples;

public class ServiceErrorNotFoundExampleProvider : IExamplesProvider<ServiceError>
{
    public ServiceError GetExamples()
    {
        return ServiceError.NotFound;
    }
}

public class ServiceErrorValidationExampleProvider : IExamplesProvider<ServiceError>
{
    public ServiceError GetExamples()
    {
        return ServiceError.Validation;
    }
}

public class ServiceErrorInternalServerErrorExampleProvider : IExamplesProvider<ServiceError>
{
    public ServiceError GetExamples()
    {
        return ServiceError.InternalServerError;
    }
}