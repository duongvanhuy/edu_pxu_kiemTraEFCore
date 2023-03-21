using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


namespace Edu.PXU.API;

public interface IJwtLib
{
	public string GenerateToken(UserIdentity model, List<string>? roles = null);
	public string GenerateRefreshToken();
	public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
}