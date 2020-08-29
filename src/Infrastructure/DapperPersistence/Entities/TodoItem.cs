using System;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Infrastructure.DapperPersistence.Common;

namespace CleanArchitecture.Infrastructure.DapperPersistence.Entities
{
    public class TodoItem : AuditableEntity
    {
        public int TodoListId { get; set; }

        public string Title { get; set; }

        public string Note { get; set; }

        public bool Done { get; set; }

        public DateTime? Reminder { get; set; }

        public PriorityLevel Priority { get; set; }
    }
}