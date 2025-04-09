
using BookShop.Application.Features.Discount.Commands.Create;
using BookShop.Application.Features.Discount.Commands.Update;

namespace BookShop.Application.Features.Discount.Mapping
{
    public static class DiscountMapper
    {
        public static E.Discount ToDiscount(CreateDiscountCommand command)
        {
            return new E.Discount
            {
                DiscountPercent = command.DiscountPercent,
                DiscountPrice = command.DiscountPrice,
                EndDate = command.EndDate,
                MaximumUseCount = command.MaximumUseCount,
                Name = command.Name,
                Priority = command.Priority,
                StartDate = command.StartDate,
                UsedCount = 0,
            };
        }


        public static E.Discount ToDiscount(E.Discount discount, UpdateDiscountCommand command)
        {
            discount.DiscountPercent = command.DiscountPercent;
            discount.DiscountPrice = command.DiscountPrice;
            discount.EndDate = command.EndDate;
            discount.MaximumUseCount = command.MaximumUseCount;
            discount.Name = command.Name;
            discount.Priority = command.Priority;
            discount.StartDate = command.StartDate;
            return discount;
        }




    }
}
