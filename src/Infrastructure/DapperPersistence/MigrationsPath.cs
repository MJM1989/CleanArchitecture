namespace CleanArchitecture.Infrastructure.DapperPersistence
{
    public class MigrationsPath
    {
        public MigrationsPath(string value) => Value = value;

        public string Value { get; }
    }
}