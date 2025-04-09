using BookShop.Application.Authorization;
using BookShop.Application.Extensions;
using BookShop.Application.Features.Discount.Mapping;
using BookShop.Domain.Common;
using BookShop.Domain.Constants;
using BookShop.Domain.Enums;
using BookShop.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Features.Discount.Commands.Update
{
    [RequiredPermission(PermissionConstants.UpdateDiscount)]
    public class UpdateDiscountCommand : IRequest<Result<Empty>>, IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int? DiscountPrice { get; set; }
        public float? DiscountPercent { get; set; }
        public int? MaximumUseCount { get; set; }
        public int Priority { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }



    public class UpdateDiscountCommandHanler : IRequestHandler<UpdateDiscountCommand, Result<Empty>>
    {
        #region constructor

        private readonly IDiscountRepository _discountRepository;
        public UpdateDiscountCommandHanler(IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        #endregion


        public async Task<Result<Empty>> Handle(UpdateDiscountCommand request, CancellationToken cancellationToken)
        {
            //GetEntity
            E.Discount discount = await _discountRepository.Get(request.Id);

            //Mapping
            discount = DiscountMapper.ToDiscount(discount, request);

            //Save
            await _discountRepository.Update(discount);


            return new Result<Empty>(Empty.New, true);
        }



    }



}
