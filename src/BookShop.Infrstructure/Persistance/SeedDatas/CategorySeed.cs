using Bogus;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.Infrastructure.Persistance.SeedDatas;

namespace BookShop.Infrstructure.Persistance.SeedDatas
{
    internal class CategorySeed
    {
        public readonly string userId;
        private readonly int layer1 = 3;
        public List<Guid> CategoryIds = new List<Guid>();
        private readonly Randomizer _randomizer = new Randomizer();
        public int Count { get; private set; } = 0;
        public CategorySeed(string userId)
        {
            this.userId = userId;
        }




        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            Category category_1;
            Category category_2;
            for (int i = 1; i <= layer1; i++)
            {
                category_1 = CreateRandom();
                for (int j = 1; j <= Random.Shared.Next(1, 3); j++)
                {
                    category_2 = CreateRandom();
                    category_2.Childs = GetCategories(Random.Shared.Next(1, 4));
                    category_1.Childs.Add(category_2);
                }
                categories.Add(category_1);
            }
            return categories;
        }

        private List<Category> GetCategories(int count)
        {
            List<Category> categories = new List<Category>();
            for (int i = 1; i <= count; i++)
            {
                categories.Add(CreateRandom());
            }
            return categories;
        }



        private Category CreateRandom()
        {
            Guid id = Guid.NewGuid();
            CategoryIds.Add(id);
            Count++;
            return new Category
            {
                Id = id,
                CreateBy = userId,
                CreateDate = DateTime.UtcNow,
                DeleteDate = null,
                DeletedBy = null,
                IsDeleted = false,
                LastModifiedBy = userId,
                LastModifiedDate = DateTime.UtcNow,
                ImageName = null,
                Title = $"Category - {Count}",
                Childs = new List<Category>(),
            };

        }


    }
}
