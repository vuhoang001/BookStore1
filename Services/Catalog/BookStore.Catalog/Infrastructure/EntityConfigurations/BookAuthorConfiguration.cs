using BookStore.Catalog.Domain.AggregateModels.BookModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Catalog.Infrastructure.EntityConfigurations;

public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
{
    public void Configure(EntityTypeBuilder<BookAuthor> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.AuthorId).IsRequired();

        builder
            .HasOne(x => x.Author)
            .WithMany(x => x.BookAuthors)
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Book)
            .WithMany(x => x.BookAuthors)
            .HasForeignKey("BookId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.Book.IsDeleted);
    }
}