namespace Edu.PXU.API;

public class RegisterDto
{
	public string Password { get; set; } = null!;

	public string ConfirmPassword { get; set; } = null!;

	public string Email { get; set; } = null!;
}