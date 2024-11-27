
using Bogus;
using Bogus.DataSets;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;
using System.Data.Common;

namespace BookShop.IntegrationTest.Application.Product.FakeData
{
    public static class ProductFaker
    {
        //public readonly Faker<Domain.Entities.Product> Faker = new Faker<Domain.Entities.Product>();
        private static readonly Lorem lorem = new Lorem();

        static Commerce Commerce = new();


        public static Domain.Entities.Product Create()
        {
            Guid id = Guid.NewGuid();   
            string identifire = id.ToString().Substring(0, 3);
            var product = Create(id);
            return product;
        }

        public static Domain.Entities.Product Create(Guid id)
        {
            string identifire = id.ToString().Substring(0, 3);
            Domain.Entities.Product product = new Domain.Entities.Product
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

        public static Domain.Entities.Product Create(Guid id, string title)
        {

            string identifire = id.ToString().Substring(0, 3);
            Domain.Entities.Product product = Create(id);
            product.Title = title;
            return product;
        }


        public static Domain.Entities.Product Create(Guid id, int price)
        {
            Domain.Entities.Product product = Create(id);
            product.Price = price;
            return product;
        }



        public static Domain.Entities.Product Create(Guid? id = null, string? title = null, int? price = null,
            ProductType? productType = null ,List<Product_Discount>? product_Discounts = null,
            bool? available = null , int? sellCount = 0)
        {
            id = id ?? Guid.NewGuid();
            string identifire = id.ToString().Substring(0, 3);
            Domain.Entities.Product product = new Domain.Entities.Product
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
            };
            return product;
        }





    }
}
