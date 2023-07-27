using System.Security.Claims;

namespace Whoof.Application.Common.Interfaces;

public interface ICurrentUserService
{
    ClaimsPrincipal CurrentUser { get; }

    string GetCurrentUserUniqueId();
}