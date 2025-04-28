using BookShop.Application.Extensions;
using BookShop.WebApi.Models.Author;
using BookShop.WebApi.Models.Review;

namespace BookShop.WebApi.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewModel ToModel(E.Review review)
        {
            string userImageName = review.User != null && review.User.ImageName != null ? review.User.ImageName : PathExtensions.User.DefaultImageName;
            return new ReviewModel
            {
                Email = review.Email ?? review.User.Email,
                Name = review.Name ?? review.User.Name,
                Score = review.Score,
                Text = review.Text,
                UserImagePath = Path.Combine(PathExtensions.User.Images , userImageName),
            };
        }

        public static List<ReviewModel> ToModel(List<E.Review> authors)
        {
            return authors.Select(a => ToModel(a)).ToList();
        }
    }
}
