using BookShop.Application.Authorization;
using BookShop.Application.Common.Request;
using BookShop.Application.Features.Discount.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.IRepositories;
using MediatR;

namespace BookShop.Application.Features.Discount.Commands.Create
{
    [RequiredPermission(PermissionConstants.Discount.Add)]
    public class CreateDiscountCommand : IRequest<Result<Empty>>, IValidatableRquest
    {
        public string Name { get; set; }
        public int? DiscountPrice { get; set; }
        public float? DiscountPercent { get; set; }
        public int? MaximumUseCount { get; set; }
        public int Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }



    public class CreateDiscountCommandHanler : IRequestHandler<CreateDiscountCommand,Result<Empty>>
    {
        #region constructor

        private readonly IDiscountRepository _discountRepository;
        public CreateDiscountCommandHanler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(CreateDiscountCommand request, CancellationToken cancellationToken)
        {
            //Mapping
            E.Discount discount = DiscountMapper.ToDiscount(request);

            //SaveEntity
            await _discountRepository.Add(discount);

            return new Result<Empty>(Empty.New, true);
        }


    }






}
