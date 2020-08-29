using CleanArchitecture.Infrastructure.DapperPersistence.Common;

namespace CleanArchitecture.Infrastructure.DapperPersistence.Entities
{
    public class TodoList : AuditableEntity
    {
        public string Title { get; set; }

        public string Colour { get; set; }
    }
}