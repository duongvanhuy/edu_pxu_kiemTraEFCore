namespace Edu.PXU.API;

public class TokenDto
{
    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime TokenExpiresIn { get; set; }

    public string TokenType { get; set; } = "Bearer";

    public string TokenExpiresMinutes { get; set; } = string.Empty;

    public string RefreshTokenExpiresMinutes { get; set; } = string.Empty;
}