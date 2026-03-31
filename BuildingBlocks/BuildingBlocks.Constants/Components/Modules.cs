namespace BuildingBlocks.Constants.Components;

public static class Modules
{
    public const string Catalog  = "bookstore.catalog";
    public const string Basket   = "bookstore.basket";
    public const string Identity = "Identity";
    public const string Gateway  = "Gateway";


    public static string ToClientName(string application, string? suffix = null)
    {
        var clientName = char.ToUpperInvariant(application[0]) + application[1..];
        return string.IsNullOrWhiteSpace(suffix) ? clientName : $"{clientName} {suffix}";
    }
}