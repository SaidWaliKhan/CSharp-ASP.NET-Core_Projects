using Jwt.Application.DTOs;
using Jwt.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jwt.WebApi.Controllers;

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
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);
        return result.Success ? Ok(result) : Unauthorized(result);
    }


    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return result.Success ? Ok(result) : Unauthorized(result);
    }



    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var claim = User.Claims.Select(c => new { c.Type, c.Value });
        return Ok(new { Message = "You are Authenticated", Claim = claim });
    }



    [Authorize(Roles = "Admin")]
    [HttpGet("Admin-Only")]
    public IActionResult Admin()
    {
        return Ok(new { Message = " Well Come back Admin" });
    }




}