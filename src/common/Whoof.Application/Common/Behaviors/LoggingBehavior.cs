using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Whoof.Application.Common.Interfaces;

namespace Whoof.Application.Common.Behaviors;

public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    private readonly ILogger<LoggingBehavior<TRequest>> _logger;
    private readonly ICurrentUserService _currentUserService;

    public LoggingBehavior(ILogger<LoggingBehavior<TRequest>> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }
    
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Request {RequestType} sent by user {UserId}",
            typeof(TRequest).Name,
            _currentUserService.GetCurrentUserUniqueId());
        
        return Task.CompletedTask;
    }
}