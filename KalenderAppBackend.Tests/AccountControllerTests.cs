using KalenderAppBackend.Controllers;
using KalenderAppBackend.Dtos.Account;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace KalenderAppBackend.Tests;

public class AccountControllerTests
{
    private readonly Mock<UserManager<AppUser>> _mockUserManager;
    private readonly Mock<SignInManager<AppUser>> _mockSignInManager;
    private readonly Mock<ICalendarRepo> _mockCalendarRepo;
    private readonly Mock<ITokenService> _mockTokenService;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        var mockUserStore = new Mock<IUserStore<AppUser>>();
        _mockUserManager = new Mock<UserManager<AppUser>>(
            mockUserStore.Object,
            null, null, null, null, null, null, null, null);

        _mockSignInManager = new Mock<SignInManager<AppUser>>(
            _mockUserManager.Object,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<AppUser>>(),
            null, null, null, null);

        _mockCalendarRepo = new Mock<ICalendarRepo>();
        _mockTokenService = new Mock<ITokenService>();

        _controller = new AccountController
            (
                _mockUserManager.Object,
                _mockTokenService.Object,
                _mockSignInManager.Object,
                _mockCalendarRepo.Object
            );
    }
    private static ClaimsPrincipal CreateClaimsPrincipal(string username)
    {
        var claims = new List<Claim>
        {
            new("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname", username)
        };

        var identity = new ClaimsIdentity(claims, "TestAuthType");
        return new ClaimsPrincipal(identity);
    }

    [Fact]
    public async Task Test_Succesful_Register()
    {
        _mockUserManager
            .Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        _mockTokenService
            .Setup(ts => ts.CreateToken(It.IsAny<AppUser>()))
            .Returns("fake-jwt-token");

        var registerDto = new RegisterDto
        {
            UserName = "test",
            Email = "test@test.com",
            Password = "password"
        };

        var result = await _controller.Register(registerDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var newUserDto = Assert.IsType<NewUserDto>(okResult.Value);
        Assert.Equal(registerDto.UserName, newUserDto.Username);
        Assert.Equal(registerDto.Email, newUserDto.Email);
        Assert.Equal("fake-jwt-token", newUserDto.Token);
    }
    [Fact]
    public async Task Test_Failed_Register()
    {
        _mockUserManager
            .Setup(um => um.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Username is required" }));

        var registerDto = new RegisterDto
        {
            UserName = "test",
            Email = "test@test.com",
            Password = "password"
        };

        var result = await _controller.Register(registerDto);

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
    }
    [Fact]
    public async Task Test_BadRequest_Register()
    {
        var registerDto = new RegisterDto
        {
            UserName = "test",
            Email = "test@test.com",
            Password = "password"
        };

        _controller.ModelState.AddModelError("UserName", "UserName is required");

        var result = await _controller.Register(registerDto);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Test_Succesful_Login()
    {
        var user = new LoginDto { Username = "testUser", Password = "12345678" };

        _mockUserManager
            .Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(new AppUser { UserName = "testUser", Email = "test@example.com" });

        _mockSignInManager
            .Setup(sm => sm.CheckPasswordSignInAsync(It.IsAny<AppUser>(), user.Password, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _mockTokenService
            .Setup(ts => ts.CreateToken(It.IsAny<AppUser>()))
            .Returns("fake-jwt-token");

        var result = await _controller.Login(user);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var newUser = Assert.IsType<NewUserDto>(okResult.Value);
        Assert.Equal("testUser", newUser.Username);
        Assert.Equal("test@example.com", newUser.Email);
        Assert.Equal("fake-jwt-token", newUser.Token);
    }
    [Fact]
    public async Task Test_Unauthorized_Login()
    {
        var user = new LoginDto { Username = "notexistingUser", Password = "12345678" };

        _mockSignInManager
            .Setup(sm => sm.CheckPasswordSignInAsync(It.IsAny<AppUser>(), user.Password, false))
            .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

        _mockTokenService
            .Setup(ts => ts.CreateToken(It.IsAny<AppUser>()))
            .Returns("fake-jwt-token");

        var result = await _controller.Login(user);
        Assert.IsType<UnauthorizedObjectResult>(result);
    }
    [Fact]
    public async Task Test_Badrequest_Login()
    {
        var user = new LoginDto { Username = "notexistingUser", Password = "12345678" };

        _controller.ModelState.AddModelError("UserName", "UserName is required");

        var result = await _controller.Login(user);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Test_Successful_AssignCalendarId()
    {
        int calendarId = 1;
        Calendar calendar = new() { Id = calendarId, Name = "testCalendar" };
        AppUser user = new() { UserName = "testName", Email = "testEmail" };
        var claimsPrincipal = CreateClaimsPrincipal("testUser");

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        _mockUserManager
            .Setup(um => um.GetUserName(It.IsAny<ClaimsPrincipal>()))
            .Returns("testName");

        _mockUserManager
            .Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockCalendarRepo
            .Setup(cr => cr.GetByIdAsync(calendarId))
            .ReturnsAsync(calendar);

        _mockUserManager
            .Setup(um => um.UpdateAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _controller.AssignCalendarId(calendarId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }
    [Fact]
    public async Task Test_BadRequest_AssignCalendarId()
    {
        int calendarId = 1;
        Calendar calendar = new() { Id = calendarId, Name = "testCalendar" };
        AppUser user = new() { UserName = "testName", Email = "testEmail" };
        var claimsPrincipal = CreateClaimsPrincipal("testUser");

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        _mockUserManager
            .Setup(um => um.GetUserName(It.IsAny<ClaimsPrincipal>()))
            .Returns("testName");

        _mockUserManager
            .Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockCalendarRepo
            .Setup(cr => cr.GetByIdAsync(calendarId))
            .ReturnsAsync(calendar);

        _mockUserManager
            .Setup(um => um.UpdateAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError()));

        var result = await _controller.AssignCalendarId(calendarId);

        Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task Test_BadRequest_Calendar_AssignCalendarId()
    {
        int calendarId = 1;
        Calendar calendar = new() { Id = calendarId, Name = "testCalendar" };
        AppUser user = new() { UserName = "testName", Email = "testEmail" };
        var claimsPrincipal = CreateClaimsPrincipal("testUser");

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };

        _mockUserManager
            .Setup(um => um.GetUserName(It.IsAny<ClaimsPrincipal>()))
            .Returns("testName");

        _mockUserManager
            .Setup(um => um.FindByNameAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mockCalendarRepo
            .Setup(cr => cr.GetByIdAsync(calendarId))
            .ReturnsAsync((Calendar)null);

        _mockUserManager
            .Setup(um => um.UpdateAsync(It.IsAny<AppUser>()))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _controller.AssignCalendarId(calendarId);

        Assert.IsType<BadRequestObjectResult>(result);
    }
}