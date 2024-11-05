using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Domain.Constants;
using MediatR;

namespace BookShop.Application.Features.Book.Commands.Create
{

    [RequiredPermission(Permissions.AddUser)]
    public class CreateBookCommand : IRequest
    {
        public string Title { get; set; }

    }




    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand>
    {
        public Task Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {

            throw new NotImplementedException();
        }
    }

}
