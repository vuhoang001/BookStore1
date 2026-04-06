using System.ComponentModel;
using BookStore.Catalog.Infrastructure;
using BuildingBlocks.Chassis.CQRS;
using BuildingBlocks.Chassis.EF.Gridify;
using BuildingBlocks.Chassis.Mapper;
using BuildingBlocks.Chassis.Response;
using BuildingBlocks.Constants.Core;
using Gridify;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Catalog.Features.Author.List;

public sealed record ListAuthorQuery(
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageIndex)]
    int PageIndex = 1,
    [property: Description("Number of items to return in a single page of results")]
    [property: DefaultValue(Pagination.DefaultPageSize)]
    int PageSize = 20,
    [property: Description("Number of items to return in a single page of results")]
    string? OrderBy = null,
    string? Filter = null
) : IQuery<PaginatedItemsViewModel>;

public class ListAuthorHandler(
    CatalogDbContext context,
    IMapper<Domain.AggregateModels.AuthorModel.Author, AuthorDto> mapper)
    : IQueryHandler<ListAuthorQuery, PaginatedItemsViewModel>
{
    public async Task<PaginatedItemsViewModel> Handle(ListAuthorQuery request, CancellationToken cancellationToken)
    {
        var filter = new QueryFilter
        {
            Filter   = request.Filter,
            OrderBy  = request.OrderBy,
            Page     = request.PageIndex,
            PageSize = request.PageSize
        }.ToGridify();

        var query = context.Authors.AsNoTracking().ApplyFiltering(filter).AsQueryable();
        var count = await query.CountAsync(cancellationToken);

        var res = await query.ApplyOrdering(filter).ApplyPaging(filter).ToListAsync(cancellationToken);


        var result = mapper.Map(res);


        return new PaginatedItemsViewModel(filter.Page, filter.PageSize, count, result);
    }
}