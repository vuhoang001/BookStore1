using System.ComponentModel;
using BookStore.Basket.Infrastructure;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.Response;
using BuildingBlocks.Constants.Core;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Basket.Features.Book.List;

public sealed record ListBookQuery(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageIndex)]
    int PageIndex,
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageSize)]
    int PageSize
) : IQuery<PaginatedItemsViewModel>;

public sealed record ListBookQueryHandler(BasketDbContext Context)
    : IQueryHandler<ListBookQuery, PaginatedItemsViewModel>
{
    public Task<PaginatedItemsViewModel> Handle(ListBookQuery request, CancellationToken cancellationToken)
    {
        // var query = Context.Books.ApplyFiltering(filter).AsNoTracking();
        //
        // var count = await query.CountAsync(cancellationToken);
        //
        // var result = await query.ApplyOrdering(filter).ApplyPaging(filter).ToListAsync(cancellationToken);
        //
        //
        // return new PaginatedItemsViewModel(filter.Page, filter.PageSize, count, result);
        return Task.FromResult(new PaginatedItemsViewModel(1, 20, 20, "hoanag13"));
    }
}