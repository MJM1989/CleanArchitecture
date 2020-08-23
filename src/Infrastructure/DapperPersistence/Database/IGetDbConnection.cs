using System.Data;

namespace CleanArchitecture.Infrastructure.DapperPersistence.Database
{
    public interface IGetDbConnection
    {
        IDbConnection Get();
    }
}