using System.Data;
using System.Data.SqlClient;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Infrastructure.Persistence.Database
{
    public class SqlConnectionGetter : IGetDbConnection
    {
        private readonly ConnectionString connectionString;

        public SqlConnectionGetter(ConnectionString connectionString)
        {
            this.connectionString = connectionString;
        }

        public IDbConnection Get()
        {
            return new SqlConnection(connectionString.Value);
        }
    }
}