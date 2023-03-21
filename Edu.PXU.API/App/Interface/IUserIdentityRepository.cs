using Edu.PXU.EntityFECore.Entity;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Edu.PXU.API.App.Interface
{
    public interface IUserIdentityRepository : IRepository<UserIdentity>
    {
        public Task<IdentityResult> SignUpAsync(RegisterDto dto);
        public Task<SignInResult> SignInAsync(UserIdentity user, string password);
        public Task<SignInResult> SignInWithOutPasswordAsync(UserIdentity user);
        public Task<bool> RevokeAll();
        public Task<TokenDto?> CreateUserToken(string email);
        public Task<UserIdentity?> FindByEmail(string email);
        public Task<UserIdentity?> FindById(string id);
        public Task<UserIdentity?> GetMe(ClaimsPrincipal user);
        public Task<bool> UpdateAsync(UserIdentity user);
        public Task<List<string>?> GetRoleByUser(UserIdentity user);
        public Task<int> CountAsync();
        public bool IsEmailExist(string email);
    }
}
