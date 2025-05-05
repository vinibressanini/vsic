using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infra.Configs.EntityConfiguration
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("tb_post");

            builder.HasKey(u => u.Id);

            builder.Property(e => e.Id)
                .HasColumnName("guid")
                .HasColumnType("uuid");

            builder.Property(p => p.Title)
                .HasColumnName("title")
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(p => p.Content)
                .HasColumnName("content")
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(p => p.Slug)
                .HasColumnName("slug")
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamptz")
                .IsRequired();

            builder.Property(p => p.PublishAt)
                .HasColumnName("publish_at")
                .HasColumnType("timestamptz");

            builder.Property(p => p.Status)
                .HasColumnName("status")
                .HasConversion<string>()
                .IsRequired();

            builder.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId);

            builder.HasMany(p => p.Categories)
                .WithMany(c => c.Posts)
                .UsingEntity(j =>
                {
                    j.ToTable("tb_post_category");
                });

        }
    }
}
