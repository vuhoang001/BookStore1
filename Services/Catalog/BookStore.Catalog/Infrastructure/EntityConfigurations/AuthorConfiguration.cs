using BookStore.Catalog.Domain.AggregateModels.AuthorModel;
using BuildingBlocks.Constants.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Catalog.Infrastructure.EntityConfigurations;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AuthorName).IsRequired().HasMaxLength(DataSchemaLength.Large);

        builder.HasIndex(e => e.AuthorName).IsUnique();
    }
}