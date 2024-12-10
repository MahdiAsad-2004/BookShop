using Bogus;
using Bogus.DataSets;
using BookShop.Domain.Entities;

namespace BookShop.IntegrationTest.Application.Category.FakeData
{
    internal static class CategoryFakeData
    {
        private static Faker<E.Category> _faker = new Faker<E.Category>();


        private static void SetRules()
        {
            _faker.RuleFor(r => r.Id, (a, b) => Guid.NewGuid());
            _faker.RuleFor(r => r.ImageName, (a, b) => a.Image.PlaceImgUrl(category:"books"));
            _faker.RuleFor(r => r.Title, (a, b) => $"category-{b.Id.ToString().Substring(0 , 2)}");
        }


        public static E.Category Create()
        {
            SetRules();
            return _faker.Generate();
        }

        
        public static List<E.Category> CreateBetween(int min , int max )
        {
            SetRules();
            return _faker.GenerateBetween(min , max);
        }




    }
}
