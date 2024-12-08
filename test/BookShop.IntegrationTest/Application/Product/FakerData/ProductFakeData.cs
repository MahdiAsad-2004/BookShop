
using Bogus;
using Bogus.DataSets;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using System.Data.Common;

namespace BookShop.IntegrationTest.Application.Product.FakeData
{
    public static class ProductFakeData
    {
        //public readonly Faker<Domain.Entities.Product> Faker = new Faker<Domain.Entities.Product>();
        private static readonly Lorem lorem = new Lorem();

        static Commerce Commerce = new();


        public static E.Product Create()
        {
            Guid id = Guid.NewGuid();
            string identifire = id.ToString().Substring(0, 3);
            var product = Create(id);
            return product;
        }

        public static List<E.Product> CreateBetween(int min, int max)
        {
            List<E.Product> products = new List<E.Product>();

            if (min <= 0 || min > max)
                return products;

            for (int i = 1; i < Random.Shared.Next(min, max + 1); i++)
            {
                products.Add(Create());
            }
            return products;
        }


        public static E.Product Create(Guid id)
        {
            string identifire = id.ToString().Substring(0, 3);
            E.Product product = new E.Product
            {
                Id = id,
                DescriptionHtml = lorem.Paragraph(),
                Title = $"TestProduct-{identifire}",
                ImageName = $"Image-{identifire}",
                Price = Random.Shared.Next(1000, 10_000_000),
                ProductType = Domain.Enums.ProductType.Book,
                NumberOfInventory = Random.Shared.Next(0, 500),
                SellCount = Random.Shared.Next(0, 100),
            };
            return product;
        }

        public static E.Product Create(Guid id, string title)
        {

            string identifire = id.ToString().Substring(0, 3);
            E.Product product = Create(id);
            product.Title = title;
            return product;
        }


        public static E.Product Create(Guid id, int price)
        {
            E.Product product = Create(id);
            product.Price = price;
            return product;
        }



        public static E.Product Create(Guid? id = null, string? title = null, int? price = null,
            ProductType? productType = null, List<Product_Discount>? product_Discounts = null,
            bool? available = null, int? sellCount = 0, List<E.Review>? reviews = null)
        {
            id = id ?? Guid.NewGuid();
            string identifire = id.ToString().Substring(0, 3);
            E.Product product = new E.Product
            {
                Id = id.Value,
                DescriptionHtml = lorem.Paragraph(),
                Title = title ?? $"TestProduct-{identifire}",
                ImageName = $"Image-{identifire}",
                Price = price ?? Random.Shared.Next(1000, 10_000_000),
                ProductType = productType ?? ProductType.Book,
                NumberOfInventory = available == false ? 0 : Random.Shared.Next(0, 500),
                SellCount = sellCount > 0 ? sellCount.Value : Random.Shared.Next(0, 100),
                Product_Discounts = product_Discounts,
                Reviews = reviews,
            };
            return product;
        }





    }
}
