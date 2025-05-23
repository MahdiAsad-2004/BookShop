﻿
using BookShop.Application.Common.Dtos;
using BookShop.Domain.Entities;
using BookShop.Domain.Enums;

namespace BookShop.Application.Features.Book.Dtos
{
    public class BookDetailDto : BaseDto
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public float? DiscountedPrice { get; set; }
        public string DescriptionHtml { get; set; }
        public string ImageName { get; set; }
        public int NumberOfInventory { get; set; }
        public int NumberOfPages { get; set; }
        public Cover Cover { get; set; }
        public Cutting Cutting { get; set; }
        public Language Language { get; set; }
        public string? Shabak { get; set; }
        public DateTime PublishYear { get; set; }
        public float? WeightInGram { get; set; }
        public float ReviewsAcceptedAverageScore { get; set; }


        public List<Review> ReviewsAccepted { get; set; }
        public E.Publisher Publisher { get; set; }
        public E.Translator? Translator { get; set; }
        public List<E.Author> Authors { get; set; }

    }
}
