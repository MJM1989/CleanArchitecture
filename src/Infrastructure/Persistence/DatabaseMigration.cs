using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using CleanArchitecture.Infrastructure.Persistence.Database;
using Dapper;

namespace CleanArchitecture.Infrastructure.Persistence
{
    public class DatabaseMigration : IMigrateDatabase
    {
        private readonly ConnectionString connectionString;
        private readonly MigrationsPath migrationsPath;

        public DatabaseMigration(ConnectionString connectionString, MigrationsPath migrationsPath)
        {
            this.connectionString = connectionString;
            this.migrationsPath = migrationsPath;
        }

        public void Execute()
        {
            using var connection = new SqlConnection(connectionString.Value);

            var executedScripts = connection.Query<string>(@"
                IF EXISTS(SELECT *
                    FROM [sys].[tables]
                   WHERE [name] = '_migrations')
                BEGIN
                    EXECUTE ( 'SELECT [Name] FROM [dbo].[_migrations]' )
                END");

            var scripts = GetScripts(executedScripts);

            foreach (var script in scripts)
            {
                connection.Execute(GetSql(script));

                connection.Execute(@"INSERT INTO [dbo].[_migrations] (Name, ExecutedOn) 
                                         VALUES (@Name, GETDATE())",
                    new {Name = script});
            }
        }

        private IOrderedEnumerable<string> GetScripts(IEnumerable<string> executedScripts)
        {
            return Directory.GetFiles(migrationsPath.Value)
                .Select(Path.GetFileNameWithoutExtension)
                .Where(fileName => !executedScripts.Contains(fileName))
                .OrderBy(s => s);
        }

        private string GetSql(string fileName) =>
            File.ReadAllText(Path.Combine(migrationsPath.Value, $"{fileName}.sql"));
    }
}
