
using Bogus;
using BookShop.Domain.Entities;

namespace BookShop.IntegrationTest.Application.Translator.FakeData
{
    public static class TranslatorFakeData
    {
        private static readonly Faker<E.Translator> _translatorFaker = new Faker<E.Translator>();


        public static E.Translator Create()
        {
            _translatorFaker.RuleFor(a => a.Id, (a, b) => Guid.NewGuid());
            _translatorFaker.RuleFor(a => a.ImageName, (a, b) => a.Person.Avatar);
            _translatorFaker.RuleFor(a => a.Name, (a, b) => a.Person.FullName);
            return _translatorFaker.Generate();
        }

    }
}
