using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
       public void Configure(EntityTypeBuilder<Expense> builder)
       {
              builder.HasKey(a => a.Id);
              builder.ToTable("expenses");

              builder.Property(a => a.Id)
                     .HasColumnName("id")
                     .HasConversion(id => id.Value, value => new EntityId<Expense>(value))
                     .ValueGeneratedOnAdd();

              builder.Property(a => a.Amount)
                     .HasColumnName("amount")
                     .IsRequired();

              builder.Property(a => a.DateTime)
                     .HasColumnName("date")
                     .IsRequired();
              
              builder.Property(a => a.UserId)
                     .HasColumnName("user_id")
                     .HasConversion(id => id.Value, value => new EntityId<User>(value))
                     .IsRequired();

              builder.Property(a => a.CategoryId)
                     .HasColumnName("category_id")
                     .HasConversion(id => id.Value, value => new EntityId<Category>(value))
                     .IsRequired();

              builder.Property(a => a.Note)
                     .HasColumnName("note")
                     .HasMaxLength(255);

              builder.Property(a => a.UpdatedAt)
                     .HasColumnName("updated_at")
                     .HasDefaultValueSql("CURRENT_TIMESTAMP")
                     .HasColumnType("timestamp with time zone");

              builder.Property(a => a.IsDeleted)
                     .HasColumnName("is_deleted")
                     .HasDefaultValue(false)
                     .IsRequired();

              builder.Property(a => a.LocalId)
                     .HasColumnName("local_id")
                     .IsRequired();
       }
}