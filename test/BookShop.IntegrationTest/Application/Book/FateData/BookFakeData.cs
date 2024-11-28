using Bogus;
using BookShop.Domain.Enums;
using BookShop.IntegrationTest.Application.Product.FakeData;
using BookShop.IntegrationTest.Application.Publisher.FakeData;

namespace BookShop.IntegrationTest.Application.Book.FakeData
{
    public class BookFakeData
    {
        private static readonly Faker<E.Book> _faker = new Faker<E.Book>();
        private static readonly Randomizer _randomizer = new Randomizer();

       


        //public static E.Book Create(Guid? id = null)
        //{
        //    id = id ?? Guid.NewGuid();
        //    return new E.Book
        //    {
        //        Id = id.Value,
        //        Cover = _randomizer.Enum<Cover>(),
        //        Cutting = _randomizer.Enum<Cutting>(),
        //        Edition = _randomizer.Int(1, 10),
        //        Language = _randomizer.Enum<Languages>(),
        //        NumberOfPages = _randomizer.Int(10, 1000),
        //        Shabak = Guid.NewGuid().ToString().Substring(0, 12),
        //        WeightInGram = _randomizer.Float(10f, 3000f),
        //    };
        //}
        

        public static E.Book Create(Guid? id = null , E.Product? product = null , List<E.Author>? authors = null,
            E.Publisher? publisher = null, E.Translator? translator = null)
        {
            id = id ?? Guid.NewGuid();
            publisher = publisher ?? PublisherFakeData.Create();
            product = product ?? ProductFakeData.Create();
            return new E.Book
            {
                Id = id.Value,
                Cover = _randomizer.Enum<Cover>(),
                Cutting = _randomizer.Enum<Cutting>(),
                Edition = _randomizer.Int(1, 10),
                Language = _randomizer.Enum<Languages>(),
                NumberOfPages = _randomizer.Int(10, 1000),
                Shabak = Guid.NewGuid().ToString().Substring(0, 12),
                WeightInGram = _randomizer.Float(10f, 3000f),
                Product = product,
                ProductId = product.Id,
                Publisher = publisher,
                PublisherId = publisher.Id,
                Authors = authors,
                Translator = translator,
            };
        }






    }
}
