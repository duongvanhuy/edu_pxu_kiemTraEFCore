using Edu.PXU.API.App.Interface;
using Edu.PXU.EntityFECore.Data;
using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Xml.Linq;

namespace Edu.PXU.API.App
{
    public class UserIdentityRepository : Repository<UserIdentity>, IUserIdentityRepository
    {
        public UserIdentityRepository(PXUDBContext context, UserManager<UserIdentity> userManager,
         SignInManager<UserIdentity> signInManager,
         IJwtLib jwtHelper,
         IConfiguration configuration) : base(context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtHelper = jwtHelper;
            _configuration = configuration;
        }

        private readonly PXUDBContext _context;
        private readonly UserManager<UserIdentity> _userManager;
        private readonly SignInManager<UserIdentity> _signInManager;
        private readonly IJwtLib _jwtHelper;
        private readonly IConfiguration _configuration;

        public async Task<IdentityResult> SignUpAsync(RegisterDto dto)
        {
            var user = new UserIdentity()
            {
                Email = dto.Email,
                UserName = $"user-{Guid.NewGuid().ToString()}",
            };

            return await _userManager.CreateAsync(user, dto.Password);
        }

        public async Task<UserIdentity?> FindById(string id)
        {
            return await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<SignInResult> SignInAsync(UserIdentity user, string password)
        {
            return await _signInManager.CheckPasswordSignInAsync(user, password, false);
        }

        public Task<SignInResult> SignInWithOutPasswordAsync(UserIdentity user)
        {
            return _signInManager.PasswordSignInAsync(user, "", false, false);
        }

        public async Task<TokenDto?> CreateUserToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return null;
            var roles = await _userManager.GetRolesAsync(user) as List<string>;

            var token = _jwtHelper.GenerateToken(user, roles);
            var refreshToken = _jwtHelper.GenerateRefreshToken();
            _ = int.TryParse(_configuration["JWT:RefreshTokenExpiresDays"], out int refreshTokenValidityInDays);
            _ = int.TryParse(_configuration["JWT:TokenExpiresMinutes"], out int tokenExpiresTime);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
            await _userManager.UpdateAsync(user);

            string tokenExpiresMinutes = _configuration["JWT:TokenExpiresMinutes"];
            string refreshTokenExpiresDays = _configuration["JWT:RefreshTokenExpiresDays"];

            return new TokenDto
            {
                AccessToken = token,
                RefreshToken = refreshToken,
                TokenExpiresIn = DateTime.Now.AddMinutes(tokenExpiresTime),
                TokenExpiresMinutes = tokenExpiresMinutes,
                RefreshTokenExpiresMinutes = (60 * 24 * Int32.Parse(refreshTokenExpiresDays)).ToString()
            };
        }

        public async Task<bool> RevokeAll()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                foreach (var user in users)
                {
                    user.RefreshToken = null;
                    await _userManager.UpdateAsync(user);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<UserIdentity?> FindByEmail(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<UserIdentity?> GetMe(ClaimsPrincipal user)
        {
            try
            {
                var email = user.FindFirstValue(ClaimTypes.Email);

                if (string.IsNullOrEmpty(email))
                {
                    return null;
                }

                var getUser = await _userManager.FindByEmailAsync(email);

                return getUser;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> UpdateAsync(UserIdentity user)
        {
            try
            {
                await _userManager.UpdateAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<string>?> GetRoleByUser(UserIdentity user)
        {
            return await _userManager.GetRolesAsync(user) as List<string>;
        }

        public new async Task<int> CountAsync()
        {
            return await _userManager.Users.CountAsync();
        }

        public bool IsEmailExist(string email)
        {
            return _userManager.Users.Any(u => u.Email == email);
        }

       
    }
}
