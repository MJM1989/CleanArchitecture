using System;
using CleanArchitecture.Application.Common.Models;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(Guid userId);

        Task<(Result Result, Guid UserId)> CreateUserAsync(string userName, string password);

        Task<Result> DeleteUserAsync(Guid userId);
    }
}
