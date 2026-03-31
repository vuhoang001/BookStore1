using BookStore.Catalog.Domain.AggregateModels.PublisherModel;
using BuildingBlocks.Constants.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookStore.Catalog.Infrastructure.EntityConfigurations;

public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
{
    public void Configure(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(p => p.Name).HasMaxLength(DataSchemaLength.Large).IsRequired();

        builder.HasIndex(e => e.Name).IsUnique();
    }
}