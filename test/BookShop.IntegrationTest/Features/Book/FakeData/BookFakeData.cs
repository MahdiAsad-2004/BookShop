using Bogus;
using Bogus.DataSets;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using BookShop.IntegrationTest.Features.Product.FakerData;
using BookShop.IntegrationTest.Features.Publisher.FakeData;

namespace BookShop.IntegrationTest.Features.Book.FakeData
{
    public class BookFakeData
    {
        private static readonly Faker<E.Book> _faker = new Faker<E.Book>();
        private static readonly Randomizer _randomizer = new Randomizer();
        private static readonly Date _date = new Date();


        private static void SetRules()
        {
            _faker.RuleFor(a => a.Id, (a, b) => Guid.NewGuid());
            _faker.RuleFor(a => a.Cover, (a, b) => a.Random.Enum<Cover>());
            _faker.RuleFor(a => a.Cutting, (a, b) => a.Random.Enum<Cutting>());
            _faker.RuleFor(a => a.Edition, (a, b) => a.Random.Int(1, 10));
            _faker.RuleFor(a => a.Language, (a, b) => a.Random.Enum<Language>());
            _faker.RuleFor(a => a.NumberOfPages, (a, b) => a.Random.Int(10, 1000));
            _faker.RuleFor(a => a.Shabak, (a, b) => Guid.NewGuid().ToString().Substring(0, 12));
            _faker.RuleFor(a => a.WeightInGram, (a, b) => a.Random.Float(10f, 3000f));
            _faker.RuleFor(a => a.WeightInGram, (a, b) => a.Random.Float(10f, 3000f));
        }


        //public static E.Book Create()
        //{
        //    E.Product product = ProductFakeData.Create();
        //    E.Publisher publisher = PublisherFakeData.Create(); 
        //    return new E.Book
        //    {
        //        Id = Guid.NewGuid(),
        //        Cover = _randomizer.Enum<Cover>(),
        //        Cutting = _randomizer.Enum<Cutting>(),
        //        Edition = _randomizer.Int(1, 10),
        //        Language = _randomizer.Enum<Languages>(),
        //        NumberOfPages = _randomizer.Int(10, 1000),
        //        Shabak = Guid.NewGuid().ToString().Substring(0, 12),
        //        WeightInGram = _randomizer.Float(10f, 3000f),
        //        Product = product,
        //        ProductId = product.Id,
        //        Publisher = publisher,
        //        PublisherId = publisher.Id,
        //        PublishYear = _date.Between(DateTime.Now.AddYears(-10) , DateTime.Now),
        //    };
        //}



        public static List<E.Book> CreateBetween(int min, int max)
        {
            List<E.Book> books = new List<E.Book>();

            if (min <= 0 || min > max)
                return books;

            for (int i = 1; i <= Random.Shared.Next(min, max + 1); i++)
            {
                books.Add(Create());
            }
            return books;
        }



        public static E.Book Create(Guid? id = null, E.Product? product = null, Author_Book[]? author_Books = null,
            E.Publisher? publisher = null, E.Translator? translator = null, DateTime? publishYear = null)
        {
            id = id ?? Guid.NewGuid();
            product = product ?? ProductFakeData.Create();
            publisher = publisher ?? PublisherFakeData.Create();
            return new E.Book
            {
                Id = id.Value,
                Cover = _randomizer.Enum<Cover>(),
                Cutting = _randomizer.Enum<Cutting>(),
                Edition = _randomizer.Int(1, 10),
                Language = _randomizer.Enum<Language>(),
                NumberOfPages = _randomizer.Int(10, 1000),
                Shabak = Guid.NewGuid().ToString().Substring(0, 12),
                WeightInGram = _randomizer.Float(10f, 3000f),
                Product = product,
                ProductId = product.Id,
                Publisher = publisher,
                PublisherId = publisher.Id,
                PublishYear = publishYear ?? _date.Between(DateTime.Now.AddYears(-10), DateTime.Now),
                Author_Books = author_Books,
                Translator = translator,
                TranslatorId = translator?.Id,
            };
        }






    }
}
