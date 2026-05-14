namespace Structure.Data.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public int ExpiresIn { get; set; }

    public string TokenType { get; set; } = "Bearer";

    public string Username { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }
}