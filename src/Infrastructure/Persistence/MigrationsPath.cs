namespace CleanArchitecture.Infrastructure.Persistence
{
    public class MigrationsPath
    {
        public MigrationsPath(string value) => Value = value;

        public string Value { get; }
    }
}