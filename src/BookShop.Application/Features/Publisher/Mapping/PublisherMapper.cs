using BookShop.Application.Features.Publisher.Commands.Create;
using BookShop.Application.Features.Publisher.Commands.Update;

namespace BookShop.Application.Features.Publisher.Mapping
{
    public static class PublisherMapper
    {
        public static E.Publisher ToPublisher(CreatePublisherCommand command)
        {
            return new E.Publisher
            {
                Title = command.Title,
            };
        }


        
        public static E.Publisher ToPublisher(E.Publisher publisher,UpdatePublisherCommand command)
        {
            publisher.Title = command.Title;
            return publisher;
        }



    }
}
