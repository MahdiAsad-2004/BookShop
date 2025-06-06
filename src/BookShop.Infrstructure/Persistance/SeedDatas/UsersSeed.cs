﻿
using Bogus;
using BookShop.Domain.Entities;
using BookShop.Domain.Identity;

namespace BookShop.Infrastructure.Persistance.SeedDatas
{
    public class UsersSeed
    {
        private int _counter = 1;
        private Faker<User> _faker = new Faker<User>();
        private static readonly string _superAdminPassword = "12345";
        public static readonly Guid SuperAdminId = Guid.Parse("8826c891-6e75-48a3-b347-0d7e21113f21");
        private readonly IPasswordHasher _passwordHasher;
        public UsersSeed(IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _faker.RuleFor(a => a.Id, b => Guid.NewGuid())
                .RuleFor(a => a.ConcurrencyStamp, b => string.Empty)
                .RuleFor(a => a.CreateBy, b => SuperAdminId.ToString())
                .RuleFor(a => a.CreateDate, b => DateTime.UtcNow)
                .RuleFor(a => a.DeleteDate, b => null)
                .RuleFor(a => a.DeletedBy, b => null)
                .RuleFor(a => a.Name, b => b.Name.FullName())
                .RuleFor(a => a.Email, (a,b) => a.Internet.Email(b.Name))
                .RuleFor(a => a.NormalizedEmail, (a, b) => b.Email)
                .RuleFor(a => a.EmailConfirmed, b => b.Random.Bool(0.8f))
                .RuleFor(a => a.IsDeleted, b => false)
                .RuleFor(a => a.ImageName, b => $"user-{_counter}.jpg")
                .RuleFor(a => a.LastModifiedBy, b => SuperAdminId.ToString())
                .RuleFor(a => a.LastModifiedDate, b => DateTime.UtcNow)
                .RuleFor(a => a.PasswordHash, (a, b) => passwordHasher.Hash(b.Id.ToString().Substring(4)))
                .RuleFor(a => a.PhoneNumber, b => b.Random.String2(11, chars: "0123456789"))
                .RuleFor(a => a.SecurityStamp, b => null)
                .RuleFor(a => a.AccessFailedCount, b => 0)
                .RuleFor(a => a.LockoutEnabled, b => false)
                .RuleFor(a => a.LockoutEnd, b => null)
                .RuleFor(a => a.Username, (a, b) => $"User - {_counter}")
                .RuleFor(a => a.NormalizedUsername, (a, b) => b.Username)
                .RuleFor(a => a.PhoneNumberConfirmed, b => b.Random.Bool(0.7f))
                .RuleFor(a => a.TwoFactorEnabled, false);
            
        }


        public User SuperAdmin()
        {
            return new User
            {
                Id = SuperAdminId,
                ConcurrencyStamp = string.Empty,
                CreateBy = SuperAdminId.ToString(),
                CreateDate = DateTime.UtcNow,
                DeleteDate = null,
                DeletedBy = null,
                Email = "MohammadMahdiAsadi2004@gmail.com",
                NormalizedEmail = "MohammadMahdiAsadi2004@gmail.com",
                EmailConfirmed = true,
                IsDeleted = false,
                ImageName = string.Empty,
                LastModifiedBy = SuperAdminId.ToString(),
                LastModifiedDate = DateTime.UtcNow,
                Name = "Mahdi Asadi",
                PasswordHash = _passwordHasher.Hash(_superAdminPassword),
                //PasswordHistories = new List<PasswordHistory>
                //{
                //    new PasswordHistory
                //    {
                //        CreateBy = SuperAdminId.ToString(),
                //        CreateDate = DateTime.UtcNow,
                //        DeleteDate = null,
                //        DeletedBy = null,
                //        Id = Guid.NewGuid(),
                //        IsDeleted = false,
                //        LastModifiedBy = SuperAdminId.ToString(),
                //        LastModifiedDate = DateTime.UtcNow,
                //        PasswordHash = "ba3253876aed6bc22d4a6ff53d8406c6ad864195ed144ab5c87621b6c233b548baeae6956df346ec8c17f5ea10f35ee3cbc514797ed7ddd3145464e2a0bab413",
                //        UserId = SuperAdminId,
                //    },
                //},
                PhoneNumber = "01234569998",
                SecurityStamp = null,
                AccessFailedCount = 0,
                LockoutEnabled = false,
                LockoutEnd = null,
                Username = "Mahdi",
                NormalizedUsername = "Mahdi",
                PhoneNumberConfirmed = true,
                TwoFactorEnabled = false,
            };
        }




        public List<User> Get()
        {
            List<User> users = new List<User>();
            for (int i = 1; i <= 20; i++)
            {
                users.Add(_faker.Generate());
                _counter++;
            }
            return users;
            return _faker.GenerateBetween(15, 20).ToList();
        }



    }
}
