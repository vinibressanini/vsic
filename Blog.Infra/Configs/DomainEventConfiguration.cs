using Blog.Domain.Events;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infra.Configs
{
    internal class DomainEventConfiguration : IEntityTypeConfiguration<DomainEvent>
    {
        public void Configure(EntityTypeBuilder<DomainEvent> builder)
        {

            builder.ToTable("tb_domain_event");

            builder.HasKey(u => u.Id);

            builder.Property(e => e.Id)
                .HasColumnName("guid")
                .HasColumnType("uuid");

            builder.Property(de => de.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.Property(de => de.ProcessedAt)
                .HasColumnName("processed_at")
                .HasColumnType("timestamp");

            builder.Property(de => de.Status)
                .HasColumnName("status")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(de => de.Event)
                .HasColumnName("event")
                .HasColumnType("varchar(50)")
                .IsRequired();

            builder.Property(de => de.Payload)
                .HasColumnName("payload")
                .HasColumnType("varchar")
                .IsRequired();
        }
    }
}
