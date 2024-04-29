using System.Security.Claims;
using Whoof.Application.Common.Interfaces;

namespace Whoof.Api.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ClaimsPrincipal CurrentUser => _httpContextAccessor.HttpContext?.User ?? throw new InvalidOperationException("Current user is not set");
    
    public string GetCurrentUserUniqueId()
    {
        var claim = CurrentUser.FindFirst("unique_id");
        return claim?.Value ?? throw new InvalidOperationException("Missing unique_id claim in JWT");
    }
}