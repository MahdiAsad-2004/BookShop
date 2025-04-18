﻿using BookShop.Application.Common.Dtos;
using BookShop.Application.Features.Product.Dtos;
using BookShop.Application.Features.Product.Queries.GetSummaries;
using BookShop.Application.Features.Product.Queries.GetSummary;
using BookShop.Domain.Common.QueryOption;
using BookShop.Domain.Enums;
using BookShop.Domain.QueryOptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region constructor

        private readonly IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }


        #endregion


        [ActionName("/")]
        public async Task<IActionResult> GetAll(
            bool? Available = null, int? startPrice = null, int? endPrice = null, byte? score = null,
            int? itemCount = null, int? pageNumber = null , ProductSortingOrder? sort = null)
        {
            PaginatedDtos<ProductSummaryDto> paginatedProductSummaries = await _mediator.Send(new GetProductSummariesQuery
            {
                Available = Available,
                AverageScore = score,
                EndPrice = endPrice,
                SortingOrder = sort,
                StartPrice = startPrice,
                Paging = new Paging(itemCount , pageNumber),
            });
            return Ok(paginatedProductSummaries);
        }




    }
}
