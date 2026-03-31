namespace BookStore.Catalog.Domain.AggregateModels.PublisherModel;

public sealed class PublisherData : List<Publisher>
{
    public PublisherData()
    {
        AddRange([
            new Publisher("Penguin Random House"),
            new Publisher("HarperCollins"),
            new Publisher("Simon & Schuster"),
            new Publisher("Hachette Book Group"),
            new Publisher("Macmillan Publishers"),
            new Publisher("Scholastic Inc."),
            new Publisher("Wiley"),
            new Publisher("Springer Nature"),
            new Publisher("Oxford University Press"),
            new Publisher("Cambridge University Press"),
        ]);
    }
}