using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Persistence.Identity.Models;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Infrastructure.Persistence.Identity.Stores
{
    public class RoleStore : IRoleStore<ApplicationRole>
    {
        private readonly IGetDbConnection getDbConnection;

        public RoleStore(IGetDbConnection getDbConnection)
        {
            this.getDbConnection = getDbConnection;
        }

        public void Dispose()
        {
            // Nothing to dispose: the connection is disposed after every request method
        }

        public async Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            var roleId = await connection.ExecuteScalarAsync<Guid>(@"
                    INSERT INTO [dbo].[ApplicationRoles]
                        ( [Name]
                        , [NormalizedName]
                        )
                    VALUES ( [@Name]
                           , [@NormalizedName]
                           )
                    SELECT cast(scope_identity() AS uniqueidentifier)");

            role.Id = roleId;
            
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            bool succeeded = await connection.DeleteAsync(role);

            return succeeded
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {Description = "Could not delete role"});
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            return connection.GetAsync<ApplicationRole>(new Guid(roleId));
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            return connection.QuerySingleOrDefaultAsync<ApplicationRole>(
                $@"SELECT * FROM [dbo].[ApplicationRoles] WHERE NormalizedName = {nameof(normalizedRoleName)}",
                new {normalizedRoleName});
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            using IDbConnection connection = getDbConnection.Get();

            var succeeded = await connection.UpdateAsync(role);

            return succeeded
                ? IdentityResult.Success
                : IdentityResult.Failed(new IdentityError {Description = "Failed to update role"});
        }
    }
}