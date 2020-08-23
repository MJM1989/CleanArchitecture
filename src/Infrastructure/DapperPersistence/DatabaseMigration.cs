using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;

namespace CleanArchitecture.Infrastructure.DapperPersistence
{
    public class DatabaseMigration
    {
        private readonly IWebHostEnvironment hostEnvironment;
        private readonly ConnectionString connectionString;
        private readonly MigrationsPath migrationsPath;

        public DatabaseMigration(IWebHostEnvironment hostEnvironment, ConnectionString connectionString, MigrationsPath migrationsPath)
        {
            this.hostEnvironment = hostEnvironment;
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
                .OrderBy(GetDateFromFileName);
        }

        private string GetSql(string fileName) =>
            File.ReadAllText(Path.Combine(migrationsPath.Value, $"{fileName}.sql"));

        private static DateTime GetDateFromFileName(string fileName)
            => DateTime.ParseExact(fileName.Substring(0, 12),
                "yyyyMMddHHmm",
                null);
    }
}
