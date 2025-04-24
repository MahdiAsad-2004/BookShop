
using Bogus;

namespace BookShop.IntegrationTest.Features.Review
{
    public static class ReviewFakeData
    {
        private static Faker<E.Review> _reviewFaker = new Faker<E.Review>();



        private static void SetRules()
        {
            _reviewFaker.RuleFor(r => r.Score, (f, s) => f.Random.Byte(1, 5));
            _reviewFaker.RuleFor(r => r.IsAccepted, (f, a) => f.Random.Bool(0.7f));
            _reviewFaker.RuleFor(r => r.Id, (f, a) => Guid.NewGuid());
            _reviewFaker.RuleFor(r => r.Text, (f, a) => f.Lorem.Sentence());
        }


        public static E.Review Create()
        {
            SetRules();
            return _reviewFaker.Generate();
        }

        public static List<E.Review> CreateBetween(int min, int max)
        {
            SetRules();
            return _reviewFaker.GenerateBetween(min, max);
        }






    }
}
