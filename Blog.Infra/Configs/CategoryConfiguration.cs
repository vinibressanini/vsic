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
                .UsingEntity<Dictionary<string, object>>(
                    "tb_post_category",
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("post_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Category>()
                        .WithMany()
                        .HasForeignKey("category_id")
                        .HasConstraintName("FK_tb_post_category_category_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasKey("post_id","category_id")
                );

        }
    }
}
