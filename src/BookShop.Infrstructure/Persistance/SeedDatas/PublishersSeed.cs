using Bogus;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Infrastructure.Persistance.SeedDatas;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    internal class PublishersSeed
    {
        public readonly string userId;
        public readonly int Counts = 10;
        private readonly Queue<int> _ints;
        private readonly Randomizer _randomizer = new Randomizer();
        public PublishersSeed(string userId)
        {
            this.userId = userId;
            _ints = new Queue<int>(Enumerable.Range(1, Counts).Reverse());
        }


        public List<Publisher> GetPublishers()
        {
            List<Publisher> publishers = new List<Publisher>();
            for (int i = 1; i <= Counts; i++)
            {
                publishers.Add(CreateRandom());
            }
            return publishers;
        }


        private Publisher CreateRandom()
        {
            return new Publisher
            {
                Books = null,
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                DeleteDate = null,
                DeletedBy = null,
                EBooks = null,
                IsDeleted = false,  
                LastModifiedBy = userId,
                LastModifiedDate = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                ImageName = string.Empty,
                Title = $"Publisher - {_ints.Dequeue()}",
            };
        }


    }
}
