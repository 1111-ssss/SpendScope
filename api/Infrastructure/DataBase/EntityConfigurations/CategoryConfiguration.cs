using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
       public void Configure(EntityTypeBuilder<Category> builder)
       {
              builder.HasKey(a => a.Id);
              builder.ToTable("categories");

              builder.Property(a => a.Id)
                     .HasColumnName("id")
                     .HasConversion(id => id.Value, value => new EntityId<Category>(value))
                     .ValueGeneratedOnAdd();

              builder.Property(a => a.Name)
                     .HasColumnName("name")
                     .IsRequired();

              builder.Property(a => a.IconUrl)
                     .HasColumnName("icon_url")
                     .HasMaxLength(255);
              
              builder.Property(a => a.Description)
                     .HasColumnName("description")
                     .HasMaxLength(200);
       }
}