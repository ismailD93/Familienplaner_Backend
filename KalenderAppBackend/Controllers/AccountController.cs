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

        var user = await _userManager.FindByNameAsync(loginDto.Username.ToLower());

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

    [HttpGet("getCalendarIdByUsername")]
    [Authorize]
    public async Task<IActionResult> GetCalendarIdByUsername()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var userName = User.GetUsername();
        var user = await _userManager.FindByNameAsync(userName);

        if (user == null)
        {
            return Unauthorized("Invalid Username");
        }

        var result = user.CalendarId;

        if (result == null) return NotFound(result);

        return Ok(result);
    }

    [HttpPost("assignCalendarId")]
    [Authorize]
    public async Task<IActionResult> AssignCalendarId(int calendarId)
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null)
        {
            return Unauthorized();
        }

        var calendar = await _calendarRepo.GetByIdAsync(calendarId);

        if (calendar == null) return BadRequest("calendar not found");

        appUser.CalendarId = calendar.Id;

        var result = await _userManager.UpdateAsync(appUser);
        if (!result.Succeeded) return BadRequest(result);

        return Ok(result);
    }

    [HttpPut("changePassword")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
    {
        if (string.IsNullOrWhiteSpace(model.OldPassword) || string.IsNullOrWhiteSpace(model.NewPassword))
        {
            return BadRequest("Both old and new passwords are required.");
        }

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null)
        {
            return Unauthorized();
        }

        var result = await _userManager.ChangePasswordAsync(appUser, model.OldPassword, model.NewPassword);
        if (!result.Succeeded) return BadRequest(result);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Password changed successfully.");
    }

    [HttpPut("changeEmail")]
    [Authorize]
    public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto model)
    {
        if (string.IsNullOrWhiteSpace(model.NewEmail))
        {
            return BadRequest("New email is required.");
        }

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null)
        {
            return Unauthorized();
        }

        var result = _userManager.SetEmailAsync(appUser, model.NewEmail);

        return Ok("Email changed successfully.");
    }

    [HttpPut("setAvatar")]
    [Authorize]
    public async Task<IActionResult> SetAvatar(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Invalid file.");
        }

        if (file.Length > 2 * 1024 * 1024) // Max 2MB
        {
            return BadRequest("File size exceeds 2MB.");
        }

        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null)
        {
            return Unauthorized();
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return BadRequest("Invalid file type. Only .jpg, .jpeg, .png allowed.");
        }

        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        appUser.AvatarUrl = $"/uploads/{fileName}";
        var result = await _userManager.UpdateAsync(appUser);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { appUser.AvatarUrl });
    }

    [HttpDelete("deleteAvatar")]
    [Authorize]
    public async Task<IActionResult> DeleteAvatar()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null)
        {
            return Unauthorized();
        }

        if (!string.IsNullOrEmpty(appUser.AvatarUrl))
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", appUser.AvatarUrl.TrimStart('/'));
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            appUser.AvatarUrl = null;
            var result = await _userManager.UpdateAsync(appUser);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
        }

        return Ok("Avatar deleted.");
    }

    [HttpGet("getAvatar")]
    [Authorize]
    public async Task<IActionResult> GetAvatar()
    {
        var username = User.GetUsername();
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null)
        {
            return Unauthorized();
        }

        if (appUser == null || string.IsNullOrEmpty(appUser.AvatarUrl))
        {
            return NotFound("No avatar found.");
        }

        return Ok(new { AvatarUrl = appUser.AvatarUrl });
    }

    [HttpGet("getUserByToken")]
    [Authorize]
    public async Task<IActionResult> GetUserByToken(string token)
    {
        var username = _tokenService.GetUsernameFromToken(token);
        var appUser = await _userManager.FindByNameAsync(username);
        if (appUser == null)
        {
            return Unauthorized();
        }

        return Ok(new GetUserDto
        {
            Id = appUser.Id,
            Email = appUser.Email,
            Username = appUser.UserName,
            Color = appUser.Color,
        });
    }
}
