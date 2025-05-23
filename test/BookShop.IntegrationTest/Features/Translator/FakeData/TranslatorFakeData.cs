﻿
using Bogus;
using BookShop.Domain.Entities;

namespace BookShop.IntegrationTest.Features.Translator.FakeData
{
    public static class TranslatorFakeData
    {
        private static readonly Faker<E.Translator> _translatorFaker = new Faker<E.Translator>();


        private static void SetRules()
        {
            _translatorFaker.RuleFor(a => a.Id, (a, b) => Guid.NewGuid());
            _translatorFaker.RuleFor(a => a.ImageName, (a, b) => a.Person.Avatar.Substring(50));
            _translatorFaker.RuleFor(a => a.Name, (a, b) => a.Person.FullName);
        }

        public static E.Translator Create()
        {
            SetRules();
            return _translatorFaker.Generate();
        }

        public static List<E.Translator> CreateBetween(int min, int max)
        {
            SetRules();
            return _translatorFaker.GenerateBetween(min, max);
        }

    }
}
