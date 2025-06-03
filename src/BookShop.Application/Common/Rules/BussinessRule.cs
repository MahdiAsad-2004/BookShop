using BookShop.Application.Common.Request;
using BookShop.Domain.Common;
using BookShop.Domain.Enums;
using BookShop.Domain.Exceptions;
using MediatR;
using System.Security.Principal;

namespace BookShop.Application.Common.Rules
{
    public abstract class BussinessRule<TRequest>
        //where TRequest : IRequest
        where TRequest : IValidatableRquest

    {
        private bool _stopOnError { get; set; }
        private bool _errorOccured { get; set; }
        protected TRequest _request { get; private set; }
        public List<ErrorDetail> ErrorDetails { get; private set; } = new List<ErrorDetail>();
        
        protected void errorOccured()
        {
            _errorOccured = true;
        }
        
        public void Confing(TRequest request , bool stopOnError)
        {
            _request = request;
        }
        public bool Stop()
        {
            return _stopOnError && _errorOccured;
        }

        protected void addErrorDetail(ErrorCode code , string target,string message)
        {
            ErrorDetails.Add(new ErrorDetail(code,target,message));
        }


    }
}
