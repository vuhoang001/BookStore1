using BookStore.Catalog.Domain.AggregateModels.BookModel;
using BuildingBlocks.Constants.Core;
using BuildingBlocks.SharedKernel.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Catalog.Infrastructure.EntityConfigurations;

public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(x => x.Id);


        builder.Property(p => p.Title).HasMaxLength(DataSchemaLength.Medium).IsRequired();

        builder.Property(p => p.Description).HasMaxLength(DataSchemaLength.Max).IsRequired();

        builder.Property(x => x.Image).HasMaxLength(DataSchemaLength.ExtraLarge);

        builder.OwnsOne(
            p => p.Price,
            e =>
            {
                e.Property(p => p.DiscountPrice).HasPrecision(18, 2);
                e.Property(p => p.OriginalPrice).HasPrecision(18, 2).IsRequired();
            }
        );

        builder.Property(p => p.AverageRating).HasDefaultValue(0.0);

        builder.Property(p => p.CreatedAt).HasDefaultValueSql(DateTimeHelper.SqlUtcNow);

        builder.Property(p => p.LastModifiedAt).HasDefaultValueSql(DateTimeHelper.SqlUtcNow);

        builder.Property(p => p.RowVersion).IsRowVersion();

        builder
            .HasOne(x => x.Category)
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        builder
            .HasOne(x => x.Publisher)
            .WithMany()
            .HasForeignKey(x => x.PublisherId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Navigation(e => e.Category).AutoInclude();

        builder.Navigation(e => e.Publisher).AutoInclude();

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}