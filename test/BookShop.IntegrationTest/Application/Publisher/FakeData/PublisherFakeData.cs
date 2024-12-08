
using Bogus;

namespace BookShop.IntegrationTest.Application.Publisher.FakeData
{
    public static class PublisherFakeData
    {
        private static Faker<E.Publisher> _publisherFaker = new Faker<E.Publisher>();


        private static void SetRules()
        {
            _publisherFaker.RuleFor(a => a.Id, (a, b) => Guid.NewGuid());
            _publisherFaker.RuleFor(a => a.ImageName, (a, b) => a.Image.PlaceImgUrl(category: "company"));
            _publisherFaker.RuleFor(a => a.Title, (a, b) => a.Company.CompanyName());
        }

        public static E.Publisher Create()
        {
            SetRules();
            return _publisherFaker.Generate();
        }


        public static List<E.Publisher> CreateBetween(int min , int max)
        {
            SetRules();
            return _publisherFaker.GenerateBetween(min , max);
        }

    }
}
