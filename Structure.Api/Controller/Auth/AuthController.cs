using Microsoft.AspNetCore.Mvc;
using Structure.Data.Common;
using Structure.Data.DTOs;
using Structure.Infrastructure.Security;

namespace Structure.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly JwtTokenGenerator _jwt;
    private readonly ILogger<AuthController> _logger;
    private readonly IConfiguration _configuration;

    public AuthController(
        JwtTokenGenerator jwt,
        ILogger<AuthController> logger,
        IConfiguration configuration)
    {
        _jwt = jwt;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>User login - validates credentials and returns JWT token</summary>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /api/auth/login
    ///     {
    ///        "username": "admin",
    ///        "password": "password123"
    ///     }
    /// </remarks>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status401Unauthorized)]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (request == null)
        {
            _logger.LogWarning("Login attempt with null request");
            return BadRequest(ApiResponse<object>.Failure(
                "Invalid login request",
                new List<string> { "Username and password are required" }));
        }

        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            _logger.LogWarning("Login attempt with empty credentials");
            return BadRequest(ApiResponse<object>.Failure(
                "Invalid credentials",
                new List<string> { "Username and password cannot be empty" }));
        }

        try
        {
            var validUsername = _configuration["Auth:Username"] ?? "admin";
            var validPassword = _configuration["Auth:Password"] ?? "password123";

            if (!ValidateCredentials(request.Username, request.Password, validUsername, validPassword))
            {
                _logger.LogWarning("Failed login attempt for username: {Username}", request.Username);
                return Unauthorized(ApiResponse<object>.Failure(
                    "Invalid username or password",
                    new List<string> { "Authentication failed" }));
            }

            var token = _jwt.GenerateToken(request.Username, GetRoleForUser(request.Username));
            var durationInMinutes = _configuration.GetValue<int>("JwtSettings:DurationInMinutes", 60);

            _logger.LogInformation("User '{Username}' logged in successfully", request.Username);

            var response = new AuthResponseDto
            {
                Token = token,
                ExpiresIn = durationInMinutes,
                TokenType = "Bearer",
                Username = request.Username,
                Expiration = DateTime.UtcNow.AddMinutes(durationInMinutes)
            };

            return Ok(ApiResponse<AuthResponseDto>.Success(response, "Login successful"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user: {Username}", request.Username);
            return StatusCode(StatusCodes.Status500InternalServerError,
                ApiResponse<object>.Failure("An error occurred during login", new List<string> { ex.Message }));
        }
    }

    /// <summary>Health check - verifies auth service is running</summary>
    [HttpGet("health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Health() => Ok(new { status = "Auth service is running" });

    private bool ValidateCredentials(string providedUsername, string providedPassword,
        string validUsername, string validPassword)
        => providedUsername == validUsername && providedPassword == validPassword;

    private string GetRoleForUser(string username)
        => username.Equals("admin", StringComparison.OrdinalIgnoreCase) ? "Admin" : "User";
}