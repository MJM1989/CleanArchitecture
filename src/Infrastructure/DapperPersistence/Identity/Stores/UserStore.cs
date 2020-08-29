using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.DapperPersistence.Database;
using CleanArchitecture.Infrastructure.DapperPersistence.Identity.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.DapperPersistence.Identity.Stores
{
    public class UserStore : IUserStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>
    {
        private readonly IGetDbConnection getDbConnection;

        public UserStore(IGetDbConnection getDbConnection)
        {
            this.getDbConnection = getDbConnection;
        }

        public void Dispose()
        {
            // Nothing to dispose: connection is disposed on every request
        }

        public async Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            var userId = await connection.ExecuteScalarAsync<Guid>(@"
                    INSERT INTO [dbo].[ApplicationUsers]
                        ( [UserName]
                        , [NormalizedUserName]
                        , [Email]
                        , [NormalizedEmail]
                        , [EmailConfirmed]
                        , [PasswordHash]
                        )
                    VALUES ( [@UserName]
                           , [@NormalizedUserName]
                           , [@Email]
                           , [@NormalizedEmail]
                           , [@EmailConfirmed]
                           , [@PasswordHash]
                           )
                    SELECT cast(scope_identity() AS uniqueidentifier)");

            user.Id = userId;
            
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            await connection.DeleteAsync(user);
            
            return IdentityResult.Success;
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            return await connection.GetAsync<ApplicationUser>(new Guid(userId));
        }

        public async Task<ApplicationUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"
                    SELECT * FROM [dbo].[ApplicationUsers] WHERE [NormalizedUserName] = @{nameof(normalizedUserName)}",
                new {normalizedUserName});
        }

        public Task<string> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(ApplicationUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(ApplicationUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            IDbConnection connection = getDbConnection.Get();

            bool succeeded = await connection.UpdateAsync<ApplicationUser>(user);

            return succeeded ? IdentityResult.Success : IdentityResult.Failed(new IdentityError { Description = "Failed to update user"});
        }

        public async Task<ApplicationUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            return await connection.QuerySingleOrDefaultAsync<ApplicationUser>($@"
                    SELECT * FROM [dbo].[ApplicationUsers] WHERE NormalizedEmail = @{nameof(normalizedEmail)}",
                new {normalizedEmail});
        }

        public Task<string> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<string> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetEmailAsync(ApplicationUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(ApplicationUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
    }
}