namespace BuildingBlocks.Chassis.Response;

public class PaginatedItemsViewModel(int pageIndex, int pageSize, int count, object data)
{
    public int PageIndex { get; private set; } = pageIndex;
    public int PageSize { get; private set; } = pageSize;
    public int Count { get; private set; } = count;
    public object Data { get; private set; } = data;
}