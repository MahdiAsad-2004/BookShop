
using Bogus;
using Bogus.DataSets;
using BookShop.Domain.Entities;

namespace BookShop.IntegrationTest.Application.Author.FakeData
{
    public static class AuthorFakeData
    {
        private static Faker<E.Author> _authorFaker = new Faker<E.Author>();


        public static E.Author Create()
        {
            _authorFaker.RuleFor(a => a.Id, (a, b) => Guid.NewGuid());
            _authorFaker.RuleFor(a => a.ImageName, (a, b) => a.Image.PlaceImgUrl(category:"author"));
            _authorFaker.RuleFor(a => a.Name, (a, b) => a.Person.FullName);
            return _authorFaker.Generate();
        }
        

        public static List<E.Author> CreateBetween(int min , int max)
        {
            _authorFaker.RuleFor(a => a.Id, (a, b) => Guid.NewGuid());
            _authorFaker.RuleFor(a => a.ImageName, (a, b) => a.Image.PlaceImgUrl(category: "author"));
            _authorFaker.RuleFor(a => a.Name, (a, b) => a.Person.FullName);
            return _authorFaker.GenerateBetween(min , max);
        }

    }
}
