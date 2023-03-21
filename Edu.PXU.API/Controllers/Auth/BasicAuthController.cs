using System.Security.Claims;
using AutoMapper;
using AutoWrapper.Extensions;
using AutoWrapper.Wrappers;
using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Edu.PXU.API;

[ApiController]
public class BasicAuthController : ControllerBase
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IJwtLib _jwtLib;
	private readonly IConfiguration _configuration;
	private readonly SignInManager<UserIdentity> _signInManager;

	public BasicAuthController(IUnitOfWork unitOfWork, IJwtLib jwtLib, IConfiguration configuration,
		SignInManager<UserIdentity> signInManager)
	{
		_unitOfWork = unitOfWork;
		_jwtLib = jwtLib;
		_configuration = configuration;
		_signInManager = signInManager;
	}

	[HttpPost]
	[AllowAnonymous]
    [Route("auth/basic/register")]

    public async Task<ApiResponse> RegisterAccount([FromBody] RegisterDto dto)
	{
		if (!ModelState.IsValid)
			throw new ApiException(ModelState.AllErrors());

		var result = await _unitOfWork.UserIdentityRepository.SignUpAsync(dto);

		if (!result.Succeeded)
			throw new ApiException(result.Errors);

		var tokenDto = await _unitOfWork.UserIdentityRepository.CreateUserToken(dto.Email);

		return new ApiResponse("User created successfully.", tokenDto);
	}

	[HttpPost]
	[AllowAnonymous]
	[Route("auth/basic/login")]
	public async Task<ApiResponse> LoginAccount([FromBody] LoginDto dto)
	{
		if (!ModelState.IsValid)
			throw new ApiException(ModelState.AllErrors());

		var user = await _unitOfWork.UserIdentityRepository.FindByEmail(dto.Email);
		if (user == null)
			throw new ApiException("Invalid login attempt.");

		var result = await _unitOfWork.UserIdentityRepository.SignInAsync(user, dto.Password);

		if (!result.Succeeded)
			throw new ApiException("Invalid login attempt.");

		var tokenDto = await _unitOfWork.UserIdentityRepository.CreateUserToken(dto.Email);

		return new ApiResponse("User logged in successfully.", tokenDto);
	}

	//[Authorize]
	//[HttpGet]
 //   [Route("auth/basic/me")]

 //   public async Task<ApiResponse> GetMe()
	//{
	//	ClaimsPrincipal currentUser = this.User;
 //       UserIdentity? user = await _unitOfWork.UserIdentityRepository.GetMe(currentUser);
	//	if (user == null)
	//		throw new ApiException("User not found.", StatusCodes.Status404NotFound);
	//	return new ApiResponse("Here your are", _mapper.Map<UserIdentity, UserResponseBasicDto>(user));
	//}

	//[HttpPost]
	//[Route(RootPath.Auth.Basic.RefreshTokenRoute)]
	//public async Task<ApiResponse> RefreshToken([FromBody] RefreshTokenDto dto)
	//{
	//	if (!ModelState.IsValid)
	//		throw new ApiException(ModelState.AllErrors());

	//	EduIdentityUser? user = _unitOfWork.UserRepository.Find<EduIdentityUser>(
	//		predicate: u => u.RefreshToken == dto.RefreshToken
	//	);

	//	if (user == null || user.RefreshToken != dto.RefreshToken || user.RefreshTokenExpiryTime < DateTime.Now)
	//		throw new ApiException("Unauthorized.", StatusCodes.Status401Unauthorized);

	//	var roles = await _unitOfWork.UserRepository.GetRoleByUser(user);
	//	var newAccessToken = _jwtLib.GenerateToken(user, roles);
	//	// var newRefreshToken = _jwtLib.GenerateRefreshToken();
	//	_ = int.TryParse(_configuration["JWT:TokenExpiresMinutes"], out int tokenExpiresTime);

	//	// user.RefreshToken = newRefreshToken;
	//	await _unitOfWork.UserRepository.UpdateAsync(user);

	//	string tokenExpiresMinutes = _configuration["JWT:TokenExpiresMinutes"];
	//	string refreshTokenExpiresDays = _configuration["JWT:RefreshTokenExpiresDays"];

	//	var tokenDto = new TokenDto
	//	{
	//		AccessToken = newAccessToken,
	//		RefreshToken = user.RefreshToken,
	//		TokenExpiresIn = DateTime.Now.AddMinutes(tokenExpiresTime),
	//		TokenExpiresMinutes = tokenExpiresMinutes,
	//		RefreshTokenExpiresMinutes = (60 * 24 * Int32.Parse(refreshTokenExpiresDays)).ToString()
	//	};

	//	return new ApiResponse("Token refreshed successfully.", tokenDto);
	//}

	//[Authorize]
	//[HttpPost]
	//[Route(RootPath.Auth.Basic.LogoutRoute)]
	//public async Task<ApiResponse> Logout()
	//{
	//	ClaimsPrincipal currentUser = this.User;
	//	EduIdentityUser? user = await _unitOfWork.UserRepository.GetMe(currentUser);

	//	if (user == null)
	//		throw new ApiException("User not found.", StatusCodes.Status404NotFound);

	//	user.RefreshToken = null;
	//	user.RefreshTokenExpiryTime = null;
	//	await _unitOfWork.UserRepository.UpdateAsync(user);

	//	return new ApiResponse("User logged out successfully.", new { });
	//}
}
