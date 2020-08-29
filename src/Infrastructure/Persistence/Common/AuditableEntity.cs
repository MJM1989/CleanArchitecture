using System;

namespace CleanArchitecture.Infrastructure.Persistence.Common
{
    public abstract class AuditableEntity : Entity
    {
        public Guid CreatedBy { get; set; }

        public DateTime Created { get; set; }

        public Guid? LastModifiedBy { get; set; }

        public DateTime? LastModified { get; set; }
    }
}