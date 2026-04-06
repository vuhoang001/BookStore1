using System.ComponentModel;

namespace BookStore.Catalog.Domain.AggregateModels.BookModel;

public enum Status : byte
{
    [Description("In Stock")]     InStock    = 0,
    [Description("Out of Stock")] OutOfStock = 1,
}