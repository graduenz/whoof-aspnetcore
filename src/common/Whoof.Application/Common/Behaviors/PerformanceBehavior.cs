using System.Diagnostics;
using MediatR;
using Microsoft.Extensions.Logging;
using Whoof.Application.Common.Interfaces;

namespace Whoof.Application.Common.Behaviors;

public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly TimeSpan _longRunningRequestDuration = TimeSpan.FromMilliseconds(500);
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
    private readonly ICurrentUserService _currentUserService;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger, ICurrentUserService currentUserService)
    {
        _logger = logger;
        _currentUserService = currentUserService;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var timer = new Stopwatch();
        timer.Start();

        var response = await next();
        
        timer.Stop();

        if (timer.Elapsed >= _longRunningRequestDuration)
        {
            _logger.LogWarning(
                "Request {RequestType} sent by user with Id {UserId} took {Duration}ms, longer than {ExpectedDuration}ms",
                typeof(TRequest).Name,
                _currentUserService.GetCurrentUserUniqueId(),
                timer.ElapsedMilliseconds,
                _longRunningRequestDuration.TotalMilliseconds);
        }

        return response;
    }
}