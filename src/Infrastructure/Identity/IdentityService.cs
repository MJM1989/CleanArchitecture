using System;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using ApplicationUser = CleanArchitecture.Infrastructure.Identity.Models.ApplicationUser;

namespace CleanArchitecture.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId.ToString());

            return user.UserName;
        }
        public async Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await userManager.CreateAsync(user, password);

            return (result.ToApplicationResult(), user.Id);
        }

        public async Task<Result> DeleteUserAsync(Guid userId)
        {
            ApplicationUser user = await userManager.FindByIdAsync(userId.ToString());

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        private async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            IdentityResult result = await userManager.DeleteAsync(user);

            return result.ToApplicationResult();
        }
    }
}
