using System.ComponentModel;
using BookStore.Catalog.Infrastructure;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.EF.Gridify;
using BuildingBlocks.Chassis.Response;
using BuildingBlocks.Constants.Core;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Catalog.Features.Publisher.List;

public record ListPublisherQuery(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageIndex)]
    int PageIndex,
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageSize)]
    int PageSize,
    [property: Description("Number of items to return in a single page of results")]
    string? OrderBy,
    string? Filter
) : IQuery<PaginatedItemsViewModel>;

public class ListPublisherHandler(CatalogDbContext context) : IQueryHandler<ListPublisherQuery, PaginatedItemsViewModel>
{
    public async
        Task<PaginatedItemsViewModel> Handle(ListPublisherQuery request, CancellationToken cancellationToken)
    {
        var filter = new QueryFilter
        {
            Filter   = request.Filter,
            OrderBy  = request.OrderBy,
            Page     = request.PageIndex,
            PageSize = request.PageSize
        }.ToGridify();

        var query = context.Publishers.AsNoTracking().ApplyFiltering(filter).AsQueryable();
        var count = await query.CountAsync(cancellationToken);

        var result = await query.ApplyOrdering(filter).ApplyPaging(filter).ToListAsync(cancellationToken);


        return new PaginatedItemsViewModel(filter.Page, filter.PageSize, count, result);
    }
}