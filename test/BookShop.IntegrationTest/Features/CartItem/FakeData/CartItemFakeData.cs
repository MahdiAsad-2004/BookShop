using System.Collections.Generic;
using System.Collections.Immutable;

namespace BookShop.IntegrationTest.Features.CartItem.FakeData
{
    internal static class CartItemFakeData
    {
        private static Faker<E.CartItem> _faker = new Faker<E.CartItem>();


        private static void SetRules(List<Guid> productIds , List<Guid> cartIds)
        {
            _faker.RuleFor(r => r.Id, (a, b) => Guid.NewGuid());
            _faker.RuleFor(r => r.ProductId, (a, b) => a.PickRandom(productIds));
            _faker.RuleFor(r => r.CartId, (a, b) => a.PickRandom(cartIds));
            _faker.RuleFor(r => r.Quantity, (a, b) => a.Random.Int(1 , 5));

        }


        public static E.CartItem Create(List<Guid> productIds, List<Guid> cartIds)
        {
            SetRules(productIds, cartIds);
            return _faker.Generate();
        }


        public static List<E.CartItem> CreateBetween(int min, int max, List<Guid> productIds,List<Guid> cartIds)
        {
            SetRules(productIds,cartIds);
            return _faker.GenerateBetween(min, max);
        }


    }
}
