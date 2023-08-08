using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Whoof.Api.Services;

namespace Whoof.Tests.Api.Services;

public class CurrentUserServiceTests
{
    [Fact]
    public void CurrentUser_WhenUserIsNull_Throws()
    {
        // Arrange
        var httpCtxAccessorMock = new Mock<IHttpContextAccessor>();
        httpCtxAccessorMock.Setup(m => m.HttpContext).Returns((HttpContext?)null);
        
        var service = new CurrentUserService(httpCtxAccessorMock.Object);
        
        // Act
        var act = () => service.CurrentUser;
        
        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Current user is not set");
    }
    
    [Fact]
    public void GetCurrentUserUniqueId_WhenClaimIsNotPresent_Throws()
    {
        // Arrange
        var user = new ClaimsPrincipal();
        
        var httpCtxMock = new Mock<HttpContext>();
        httpCtxMock.Setup(m => m.User).Returns(user);
        
        var httpCtxAccessorMock = new Mock<IHttpContextAccessor>();
        httpCtxAccessorMock.Setup(m => m.HttpContext).Returns(httpCtxMock.Object);
        
        var service = new CurrentUserService(httpCtxAccessorMock.Object);

        // Act
        var act = () => service.GetCurrentUserUniqueId();

        // Assert
        act.Should()
            .Throw<Exception>()
            .WithMessage("Missing unique_id claim in JWT");
    }
}