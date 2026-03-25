namespace BuildingBlocks.Chassis.Response;

public class QueryFilter
{
    public string? Filter { get; init; }
    public string? OrderBy { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}