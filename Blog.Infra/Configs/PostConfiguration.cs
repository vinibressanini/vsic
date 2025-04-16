using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infra.Configs
{
    internal class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("tb_post");

            builder.HasKey(p => p.Id).HasName("id");

            builder.Property(p => p.Title)
                .HasColumnName("title")
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(p => p.Content)
                .HasColumnName("content")
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .IsRequired();
            
            builder.Property(p => p.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("timestamp");

            builder.Property(p => p.PublishAt)
                .HasColumnName("publish_at")
                .HasColumnType("timestamp");

            builder.Property(p => p.Status)
                .HasColumnName("status")
                .HasColumnType("smallserial")
                .IsRequired();

            builder.HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId);

            builder.HasMany(p => p.Categories)
                .WithMany(c => c.Posts)
                .UsingEntity(j =>
                {
                    j.ToTable("post_category");
                });

        }
    }
}
