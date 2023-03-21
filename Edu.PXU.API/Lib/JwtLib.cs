using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Edu.PXU.API;

public class JwtLib : IJwtLib
{
	private readonly IConfiguration _configuration;

	public JwtLib(IConfiguration configuration)
	{
		_configuration = configuration;
	}

	public string GenerateToken(UserIdentity model, List<string>? roles = null)
	{
		var authClaims = new List<Claim>
		{
			new Claim(ClaimTypes.Email, model.Email),
			new Claim(ClaimTypes.Name, model.UserName),
			new Claim("UserId", model.Id.ToString()),
			new Claim("Email", model.Email),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		if (roles != null)
		{
			foreach (var role in roles)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, role));
			}
		}

		var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));
		_ = int.TryParse(_configuration["JWT:TokenExpiresMinutes"], out int tokenExpiresTimeMinutes);

		var token = new JwtSecurityToken(
			issuer: _configuration["JWT:ValidIssuer"],
			audience: _configuration["JWT:ValidAudience"],
			expires: DateTime.Now.AddMinutes(tokenExpiresTimeMinutes),
			claims: authClaims,
			signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
		);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}

	public string GenerateRefreshToken()
	{
		var randomNumber = new byte[64];
		using var rng = RandomNumberGenerator.Create();
		rng.GetBytes(randomNumber);
		return Convert.ToBase64String(randomNumber);
	}

	public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
	{
		var tokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidAudience = _configuration["JWT:ValidAudience"],
			ValidIssuer = _configuration["JWT:ValidIssuer"],
			ValidateLifetime = false,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!))
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
		if (securityToken is not JwtSecurityToken jwtSecurityToken ||
		    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
			    StringComparison.InvariantCultureIgnoreCase))
			throw new SecurityTokenException("Invalid token");
		return principal;
	}
}