
using Bogus;
using Bogus.DataSets;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.IntegrationTest.Features.Author.FakeData
{
    public static class AuthorFakeData
    {
        private static Faker<E.Author> _authorFaker = new Faker<E.Author>();


        private static void SetRules()
        {
            _authorFaker.RuleFor(a => a.Id, (a, b) => Guid.NewGuid());
            _authorFaker.RuleFor(a => a.ImageName, (a, b) => a.Image.PlaceImgUrl(category: "author"));
            _authorFaker.RuleFor(a => a.Name, (a, b) => a.Person.FullName);
            _authorFaker.RuleFor(a => a.Gender, (a, b) => a.Random.Enum<Gender>());
        }

        public static E.Author Create()
        {
            SetRules();
            return _authorFaker.Generate();
        }


        public static List<E.Author> CreateBetween(int min, int max)
        {
            SetRules();
            return _authorFaker.GenerateBetween(min, max);
        }

        public static List<E.Author_Book> CreateBetween(int min, int max, bool a)
        {
            SetRules();
            List<Author_Book> author_books = new();
            foreach (var author in _authorFaker.GenerateBetween(min, max))
            {
                author_books.Add(new Author_Book
                {
                    Author = author,
                });
            }
            return author_books;
        }


    }
}
