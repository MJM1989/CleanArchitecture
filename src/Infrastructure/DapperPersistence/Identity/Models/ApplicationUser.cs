using System;

namespace CleanArchitecture.Infrastructure.DapperPersistence.Identity.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
 
        public string UserName { get; set; }
 
        public string NormalizedUserName { get; set; }
 
        public string PasswordHash { get; set; }
    }
}