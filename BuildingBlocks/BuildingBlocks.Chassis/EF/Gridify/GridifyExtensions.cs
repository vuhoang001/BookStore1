using BuildingBlocks.Chassis.Response;
using Gridify;

namespace BuildingBlocks.Chassis.EF.Gridify;

public static class GridifyExtensions
{
    public static GridifyQuery ToGridify(this QueryFilter filter)
    {
        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize switch
        {
            <= 0  => 20,
            > 100 => 100,
            _     => filter.PageSize
        };

        return new GridifyQuery
        {
            Filter   = filter.Filter,
            OrderBy  = filter.OrderBy,
            Page     = page,
            PageSize = pageSize
        };
    }
}