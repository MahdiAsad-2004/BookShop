using Bogus;
using BookShop.Domain.Entities;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    internal class DiscountSeed
    {
        public readonly string userId;
        public List<Guid> DiscountIds = new List<Guid>();
        private readonly Randomizer _randomizer = new Randomizer();
        //public int Count { get; private set; } = 30;
        private int counter = 0;
        public DiscountSeed(string userId)
        {
            this.userId = userId;
        }




        //private List<Discount> Get()
        //{
        //    List<Discount> discounts = new List<Discount>();
        //    for (int i = 1; i <= Count; i++)
        //    {
        //        discounts.Add(CreateRandom());
        //    }
        //    return discounts;
        //}


        public List<Product_Discount> CreateRandom(int count)
        {
            DateTime now = DateTime.UtcNow;
            List<Product_Discount> Product_Discounts = new List<Product_Discount>();
            for (int i = 1; i <= count; i++)
            {
                Product_Discounts.Add(new Product_Discount
                {
                    CreateBy = userId,
                    CreateDate = now,
                    DeleteDate = null,
                    DeletedBy = null,
                    Discount = CreateRandom(),
                    Id = Guid.NewGuid(),
                    LastModifiedBy = userId,
                    LastModifiedDate = now,
                    IsDeleted = false,
                });
            }
            return Product_Discounts;
        }


        private Discount CreateRandom()
        {
            Guid id = Guid.NewGuid();
            DiscountIds.Add(id);
            counter++;
            bool isPercent = _randomizer.Bool(0.7f);
            DateTime? startDate = _randomizer.Bool(0.7f) ? DateTime.UtcNow.AddDays(Random.Shared.Next(-3, 3)) : null;
            DateTime? endDate = _randomizer.Bool(0.7f) ? DateTime.UtcNow.AddDays(Random.Shared.Next(-3, 3)) : null;
            return new Discount
            {
                Id = id,
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                DeleteDate = null,
                DeletedBy = null,
                IsDeleted = false,
                LastModifiedBy = userId,
                LastModifiedDate = DateTime.UtcNow,
                DiscountPercent = isPercent ? _randomizer.Int(1, 99) : null,
                DiscountPrice = !isPercent ? _randomizer.Int(1_000, 100_000) : null,
                EndDate = endDate,
                MaximumUseCount = _randomizer.Int(1, 10),
                Name = $"Discount-{counter}",
                Priority = _randomizer.Int(1, 5),
                StartDate = startDate,
                UsedCount = _randomizer.Int(0, 5),
            };

        }


    }
}
