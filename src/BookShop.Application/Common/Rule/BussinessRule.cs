
using BookShop.Domain.Exceptions;
using MediatR;
using System.Security.Principal;

namespace BookShop.Application.Common.Rule
{
    public abstract class BussinessRule<TRequest> 
        where TRequest : IRequest

    {
        protected bool _errorOccured { get; private set; }
        protected void ErrorOccured()
        {
            _errorOccured = true;
        }
        protected bool MustStop(bool stopOnError)
        {
            return stopOnError && _errorOccured;
        }



        public List<ValidationError> ValidationErrors { get; private set; } = new List<ValidationError>();

        public abstract Task CheckRules(TRequest request,bool stopOnError);





    }
}
