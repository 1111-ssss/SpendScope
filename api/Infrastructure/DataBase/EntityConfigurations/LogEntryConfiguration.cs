using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
{
       public void Configure(EntityTypeBuilder<LogEntry> builder)
       {
              builder.HasKey(l => l.Id);
              builder.ToTable("log_entries");

              builder.Property(l => l.Id)
                     .HasColumnName("id")
                     .HasConversion(id => id.Value, value => new EntityId<LogEntry>(value))
                     .ValueGeneratedOnAdd();

              builder.Property(l => l.Timestamp)
                     .HasColumnName("timestamp")
                     .HasDefaultValueSql("CURRENT_TIMESTAMP")
                     .HasColumnType("timestamp with time zone")
                     .IsRequired();

              builder.Property(l => l.Level)
                     .HasColumnName("level")
                     .HasMaxLength(20)
                     .IsRequired();

              builder.Property(l => l.Message)
                     .HasColumnName("message")
                     .HasMaxLength(255)
                     .IsRequired();

              builder.Property(l => l.Exception)
                     .HasColumnName("exception")
                     .HasMaxLength(255);
       }
}