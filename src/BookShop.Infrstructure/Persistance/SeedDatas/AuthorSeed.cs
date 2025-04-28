
using Bogus;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    internal class AuthorSeed
    {
        public readonly string userId;
        public List<Guid> AuthorIds = new List<Guid>();
        private readonly Randomizer _randomizer = new Randomizer();
        public int Count { get; private set; } = 20;
        private int counter = 0;
        public AuthorSeed(string userId)
        {
            this.userId = userId;
        }



        public List<Author> Get()
        {
            List<Author> Authors = new List<Author>();
            for (int i = 1; i <= Count; i++)
            {
                Authors.Add(CreateRandom());
            }
            return Authors;
        }


    
        private Author CreateRandom()
        {
            Guid id = Guid.NewGuid();
            AuthorIds.Add(id);
            counter++;
            return new Author
            {
                Id = id,
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                DeleteDate = null,
                DeletedBy = null,
                IsDeleted = false,
                LastModifiedBy = userId,
                LastModifiedDate = DateTime.UtcNow,
                Name = $"Author-{counter}",
                Gender = _randomizer.Enum<Gender>(),
                ImageName = null,
            };

        }












        public static Author_Book RandomAuthor_Book(string userId)
        {
            DateTime now = DateTime.UtcNow;
            return new Author_Book
            {
                Id = Guid.NewGuid(),
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                DeleteDate = null,
                DeletedBy = null,
                IsDeleted = false,
                LastModifiedBy = userId,
                LastModifiedDate = DateTime.UtcNow,
            };
        }










    }
}
