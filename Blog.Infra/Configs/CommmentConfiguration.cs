using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infra.Configs
{
    internal class CommmentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.ToTable("tb_comment");

            builder.HasKey(u => u.Id).HasName("id");

            builder.Property(c => c.Content)
                .HasColumnName("content")
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(c => c.ParentId).HasColumnName("parent_id");

            builder.Property(c => c.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("timestamp")
                .IsRequired();

            builder.HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId);

            builder.HasOne(c => c.Author)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.AuthorId);

        }
    }
}
