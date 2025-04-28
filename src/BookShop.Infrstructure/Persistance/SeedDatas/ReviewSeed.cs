
using Bogus;
using Bogus.DataSets;
using BookShop.Domain.Entities;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    internal class ReviewSeed
    {
        private readonly Guid[] userIds;
        private readonly Guid[] productIds;
        private Randomizer _randomizer;
        private Lorem _lorem;
        public ReviewSeed(Guid[] userIds, Guid[] productIds)
        {
            this.userIds = userIds;
            this.productIds = productIds;
            _randomizer = new Randomizer();
            _lorem = new Lorem();
        }




        public List<Review> Get()
        {
            List<Review> reviews = new List<Review>();
            int reviewCount = Random.Shared.Next(0, 6);
            foreach (var productId in productIds)
            {
                reviewCount = Random.Shared.Next(0, 6);
                for (int i = 1; i <= reviewCount; i++)
                {
                    reviews.Add(CreateRandom(productId));
                }
            }
            return reviews;
        }


        private Review CreateRandom(Guid productId)
        {
            Guid userId = _randomizer.ArrayElement(userIds);
            DateTime now = DateTime.UtcNow;
            return new Review
            {
                CreateBy = userId.ToString(),
                CreateDate = now,
                DeleteDate = null,
                DeletedBy = null,
                Email = null,
                Id = Guid.NewGuid(),
                IsAccepted = true,
                IsDeleted = false,
                LastModifiedBy = userId.ToString(),
                LastModifiedDate = now,
                Name = null,
                ProductId = productId,
                Score = _randomizer.Byte(1, 5),
                Text = _lorem.Sentence(_randomizer.Int(10, 50)),
                UserId = userId,
            };
        }





    }
}
