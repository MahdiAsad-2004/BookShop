﻿using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.Domain.QueryOptions
{
    public class BookQueryOption : IQueryOption<Book,Guid>
    {
        public bool IncludeProduct { get; init; } = false;
        public bool IncludeReviews { get; init; } = false;
        public bool IncludeDiscounts { get; init; } = false;
        public bool IncludeAuthors { get; init; } = false;
        public bool IncludeTranslator { get; init; } = false;
        public bool IncludePublisher { get; init; } = false;
        public int? Product_StartPrice { get; init; } = null;
        public int? Product_EndPrice { get; init; } = null;
        public bool? Product_Available { get; init; } = null;
        public byte? Product_AverageScore { get; init; } = null;
        public DateTime? StartPublishYear { get; init; } = null;
        public DateTime? EndPublishYear { get; init; } = null;
        public Guid? CategoryId { get; init; } = null;
    }


    public enum BookSortingOrder
    {
        Newest, Oldest,
        Product_HighestPrice, Product_LowestPrice,
        Product_HighestDiscount, Product_LowestDiscount,
        Product_HighestSellCount, Product_LowestSellCount,
        Product_AlphabetDesc, Product_AlphabetAsce,
        PublishYearAsce ,PublishYearDesc   
    }

}
