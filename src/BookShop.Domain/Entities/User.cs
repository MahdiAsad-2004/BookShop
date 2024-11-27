﻿using BookShop.Domain.Common.Entity;

namespace BookShop.Domain.Entities
{
    public class User : Entity<Guid> 
    {
        public string Name { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUsername { get; set; }
        public string PasswordHash { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string? SecurityStamp { get; set; }
        public string ImageName { get; set; }
        public int AccessFailedCount { get; set; }
        public bool TwoFactorEnabled { get; set; }




        public IList<UserClaim> UserClaims { get; set; }
        public IList<User_Role> User_Roles { get; set; }
        public IList<AuditLog> AuditLogs { get; set; }
        public IList<Favorite> Favorites { get; set; }
        public IList<Review> Reviews { get; set; }
        public IList<PasswordHistory> PasswordHistories { get; set; }
        public IList<UserToken> UserTokens { get; set; }
        public IList<User_Permission> User_Permissions { get; set; }

    }
}
