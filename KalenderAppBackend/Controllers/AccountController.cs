using KalenderAppBackend.Dtos.Account;
using KalenderAppBackend.Extensions;
using KalenderAppBackend.Interfaces;
using KalenderAppBackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Rewrite;

namespace KalenderAppBackend.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly ICalendarRepo _calendarRepo;

    public AccountController(UserManager<AppUser> userManager
        , ITokenService tokenService
        , SignInManager<AppUser> signInManager
        , ICalendarRepo calendarRepo)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _calendarRepo = calendarRepo;
    }

    [HttpPut("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            AppUser appUser = new()
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
            };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password);

            if (createdUser.Succeeded)
            {
                return Ok(
                         new NewUserDto
                         {
                             Username = appUser.UserName,
                             Email = appUser.Email,
                             Token = _tokenService.CreateToken(appUser)
                         });
            }
            else
            {
                return StatusCode(500, createdUser.Errors);
            }
        }
        catch (Exception e)
        {
            return StatusCode(500, e);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = _userManager.Users.FirstOrDefault(x => x.UserName == loginDto.Username.ToLower());

        if (user == null) return Unauthorized("Invalid Username!");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded) return Unauthorized("Username not found and/or Password incorrect!");

        return Ok(
            new NewUserDto
            {
                Username = user.UserName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user)
            });
    }

    [HttpPost("assignCalendarId")]
    [Authorize]
    public async Task<IActionResult> AssignCalendarId(int calendarId)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        var calendar = await _calendarRepo.GetByIdAsync(calendarId);

        if (calendar == null) return BadRequest("calendar not found");

        appUser.CalendarId = calendar.Id;

        var result = await _userManager.UpdateAsync(appUser);
        if (!result.Succeeded) return BadRequest(result);

        return Ok(result);
    }
}
