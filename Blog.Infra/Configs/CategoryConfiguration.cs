using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infra.Configs
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("tb_category");

            builder.HasKey(u => u.Id);

            builder.Property(e => e.Id)
                .HasColumnName("guid")
                .HasColumnType("uuid");

            builder.Property(c => c.Name)
                .HasColumnName("name")
                .HasColumnType("varchar")
                .HasMaxLength(50)
                .IsRequired();

            builder.HasMany(c => c.Posts)
                .WithMany(p => p.Categories)
                .UsingEntity(j =>
                {
                    j.ToTable("tb_post_category");
                });

        }
    }
}
