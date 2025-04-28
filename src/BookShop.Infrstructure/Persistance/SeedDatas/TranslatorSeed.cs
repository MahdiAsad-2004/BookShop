using Bogus;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Infrastructure.Persistance.SeedDatas;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    internal class TranslatorsSeed
    {
        public readonly string userId;
        public readonly int Counts = 10;
        private readonly Queue<int> _ints;
        private readonly Randomizer _randomizer = new Randomizer();
        public TranslatorsSeed(string userId)
        {
            this.userId = userId;
            _ints = new Queue<int>(Enumerable.Range(1, Counts).Reverse());
        }


        public List<Translator> Get()
        {
            List<Translator> Translators = new List<Translator>();
            for (int i = 1; i <= Counts; i++)
            {
                Translators.Add(CreateRandom());
            }
            return Translators;
        }


        private Translator CreateRandom()
        {
            return new Translator
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
                ImageName = null,
                Name = $"Translator - {_ints.Dequeue()}",
                Gender = _randomizer.Enum<Gender>(),
            };
        }


    }
}
