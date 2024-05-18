using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vesta_api.Database.Context;
using vesta_api.Database.Models;
using vesta_api.Database.Models.View;
using vesta_api.Database.Models.View.Requests;

namespace vesta_api.Controllers;

[Route("api/authentication")]
[ApiController]
[Produces("application/json")]
[Authorize]
public class AuthenticationController(VestaContext context) : ControllerBase
{
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost("register"), AllowAnonymous] // In the future replace AllowAnonymous with Authorize(Roles = "admin")
    public async Task<IActionResult> Register(CreateUserRequest request)
    {
        if (context.Users.Any(u => u.Username == request.Username))
        {
            return Conflict("Username already exist");
        }

        CreatePasswordHash(request.Password, out var passwordHash, out var passwordKey);

        var user = new User
        {
            Username = request.Username,
            PasswordHash = passwordHash,
            PasswordKey = passwordKey,
            Role = request.Role,
            IsActive = true,
            EmployeeId = request.EmployeeId
        };

        context.Users.Add(user);

        await context.SaveChangesAsync();

        return Created();
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await context.Users
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Username == request.Username);

        if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordKey))
        {
            return Unauthorized("Wrong username or password");
        }

        if (!user.IsActive)
        {
            return Unauthorized("User is deactivated");
        }

        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.Username),
            new(ClaimsIdentity.DefaultRoleClaimType, user.Role)
        };

        var id = new ClaimsIdentity(
            claims,
            "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType
        );

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));

        return Ok("User was authorized");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("logout"), Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok("User was deauthorized");
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("me"), Authorize]
    public Task<IActionResult> GetMe()
    {
        var username = User.FindFirst(c => c.Type == ClaimsIdentity.DefaultNameClaimType)!.Value;
        var user = context.Users
            .Include(u => u.Employee)
            .First(u => u.Username == username);

        return Task.FromResult<IActionResult>(Ok(new
        {
            firstName = user.Employee.FirstName,
            lastName = user.Employee.LastName,
            patronymic = user.Employee.Patronymic,
            role = user.Role
        }));
    }

    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("check_authentication"), AllowAnonymous]
    public Task<IActionResult> CheckAuthentication()
    {
        return Task.FromResult<IActionResult>(
            Ok(User.FindFirst(c => c.Type == ClaimsIdentity.DefaultNameClaimType) != null));
    }

    [HttpPost("change_password"), Authorize(Roles = "admin")]
    public async Task<IActionResult> ChangePassword(EditPasswordRequest passwordRequest)
    {
        var user = await context.Users.FirstOrDefaultAsync(user => user.Id == passwordRequest.UserId);

        if (user == null)
        {
            return await Task.FromResult<IActionResult>(NotFound());
        }

        CreatePasswordHash(passwordRequest.Password, out var passwordHash, out var passwordKey);

        user.PasswordHash = passwordHash;
        user.PasswordKey = passwordKey;

        context.Entry(user).State = EntityState.Modified;

        await context.SaveChangesAsync();

        return await Task.FromResult<IActionResult>(Ok("Password was changed"));
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordKey)
    {
        using var hmac = new HMACSHA512();
        passwordKey = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordKey)
    {
        using var hmac = new HMACSHA512(passwordKey);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computeHash.SequenceEqual(passwordHash);
    }
}