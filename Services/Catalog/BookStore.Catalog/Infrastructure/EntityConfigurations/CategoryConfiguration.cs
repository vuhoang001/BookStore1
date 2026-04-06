using BookStore.Catalog.Domain.AggregateModels.CategoryModel;
using BuildingBlocks.Constants.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Catalog.Infrastructure.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CategoryName).HasMaxLength(DataSchemaLength.Large).IsRequired();
        builder.HasIndex(x => x.CategoryName).IsUnique();
    }
}