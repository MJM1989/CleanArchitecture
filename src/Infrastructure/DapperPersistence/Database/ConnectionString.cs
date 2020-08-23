namespace CleanArchitecture.Infrastructure.DapperPersistence.Database
{
    public sealed class ConnectionString
    {
        public ConnectionString(string value) => Value = value;

        public string Value { get; }
    }
}
