using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.DataBase.EntityConfigurations;

public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
{
       public void Configure(EntityTypeBuilder<LogEntry> builder)
       {
              builder.ToTable("logs");

              builder.HasKey(l => l.Id);

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
                     .HasColumnType("integer")
                     .IsRequired();

              builder.Property(l => l.Message)
                     .HasColumnName("message")
                     .IsRequired();

              builder.Property(l => l.Exception)
                     .HasColumnName("exception");

              builder.Property(l => l.MessageTemplate)
                     .HasColumnName("message_template");

              builder.Property(l => l.Properties)
                     .HasColumnName("properties")
                     .HasColumnType("jsonb");
              
              builder.Property(l => l.LogEvent)
                     .HasColumnName("log_event")
                     .HasColumnType("jsonb");
       }
}