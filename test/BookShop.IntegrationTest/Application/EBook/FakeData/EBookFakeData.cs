using Bogus;
using Bogus.DataSets;
using BookShop.Domain.Enums;
using BookShop.Domain.Entities;
using BookShop.IntegrationTest.Application.Product.FakeData;
using BookShop.IntegrationTest.Application.Publisher.FakeData;

namespace BookShop.IntegrationTest.Application.EBook.FakeData
{
    public class EBookFakeData
    {
        private static readonly Faker<E.EBook> _faker = new Faker<E.EBook>();
        private static readonly Randomizer _randomizer = new Randomizer();
        private static readonly Date _date = new Date();


        private static void SetRules()
        {
            _faker.RuleFor(a => a.Id, (a, b) => Guid.NewGuid());
            _faker.RuleFor(a => a.Edition, (a, b) => a.Random.Int(1, 10));
            _faker.RuleFor(a => a.Language, (a, b) => a.Random.Enum<Language>());
            _faker.RuleFor(a => a.NumberOfPages, (a, b) => a.Random.Int(10, 1000));
            _faker.RuleFor(a => a.FileFormat, (a, b) => a.Random.Enum<EBookFileFormat>());
            _faker.RuleFor(a => a.FileName, (a, b) => a.Random.String2(20));
            _faker.RuleFor(a => a.FileSize_KB, (a, b) => a.Random.Int(1000, 10_000));
            _faker.RuleFor(a => a.PublishYear, (a, b) => DateTime.UtcNow.AddDays(_randomizer.Int(-100, -10)));
        }




        public static List<E.EBook> CreateBetween(int min, int max)
        {
            List<E.EBook> books = new List<E.EBook>();

            if (min <= 0 || min > max)
                return books;

            for (int i = 1; i <= Random.Shared.Next(min, max + 1); i++)
            {
                books.Add(Create());
            }
            return books;
        }



        public static E.EBook Create(Guid? id = null, E.Product? product = null, Author_EBook[]? author_EBooks = null,
            E.Publisher? publisher = null, E.Translator? translator = null, DateTime? publishYear = null)
        {
            id = id ?? Guid.NewGuid();
            product = product ?? ProductFakeData.Create();
            publisher = publisher ?? PublisherFakeData.Create();
            return new E.EBook
            {
                Id = id.Value,
                Edition = _randomizer.Int(1, 10),
                Language = _randomizer.Enum<Language>(),
                NumberOfPages = _randomizer.Int(10, 1000),
                Product = product,
                ProductId = product.Id,
                Publisher = publisher,
                PublisherId = publisher.Id,
                PublishYear = publishYear ?? _date.Between(DateTime.Now.AddYears(-10), DateTime.Now),
                Author_EBooks = author_EBooks,
                Translator = translator,
                TranslatorId = translator?.Id,
                FileFormat = _randomizer.Enum<EBookFileFormat>(),
                FileName = _randomizer.String2(20),
                FileSize_KB = _randomizer.Int(1_000, 10_000),
            };
        }






    }
}
