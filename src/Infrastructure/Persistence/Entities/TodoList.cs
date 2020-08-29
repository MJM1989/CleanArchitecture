using System.Collections.Generic;
using CleanArchitecture.Infrastructure.Persistence.Common;

namespace CleanArchitecture.Infrastructure.Persistence.Entities
{
    public class TodoList : AuditableEntity
    {
        public string Title { get; set; }

        public string Colour { get; set; }
        public IEnumerable<TodoItem> Items { get; set; }
    }
}