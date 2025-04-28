using BookShop.Application.Extensions;
using BookShop.WebApi.Models.Publisher;

namespace BookShop.WebApi.Mappers
{
    public static class PublisherMapper
    {
        public static PublisherModel ToModel(E.Publisher publisher)
        {
            string imageName = publisher.ImageName != null ? publisher.ImageName : PathExtensions.Publisher.DefaultImageName;
            return new PublisherModel(publisher.Id, publisher.Title, Path.Combine(PathExtensions.Publisher.Images , imageName));
        }



    }
}
