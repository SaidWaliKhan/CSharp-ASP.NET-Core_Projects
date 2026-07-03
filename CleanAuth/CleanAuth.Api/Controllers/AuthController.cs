using System.IdentityModel.Tokens.Jwt;
using CleanAuth.Application.DTOs;
using CleanAuth.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CleanAuth.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        if (!result.Success)
        {
            return BadRequest(new
            {
                Message = result.ErrorMessage
            });
        }

        return Ok(new
        {
            Message = "User registered successfully",
            result.Data
        });

    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        if (!result.Success)
        {
            return BadRequest(new
            {
                Message = result.ErrorMessage
            });
        }

        return Ok(new
        {
            Message = "User logged in successfully",
            result.Data
        });
    }


    // A protected endpoint just to prove the token actually works.
    [Authorize]
    [HttpGet("protected")]
    
    public IActionResult Protected()
    {
        var email = User.FindFirst(JwtRegisteredClaimNames.Email)?.Value;
        return Ok(new
        {
            Message = "You have access to this protected endpoint.",
            Email = email
        });
    }

}