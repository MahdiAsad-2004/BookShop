using BookShop.Application.Common.Rule;
using BookShop.Domain.IRepositories;

namespace BookShop.Application.Features.Book.Commands.Create
{
    public class CreateBookCommandRule : BussinessRule
    {
        private readonly IBookRepository _bookRepository;
        public CreateBookCommandRule(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }











    }

}
