using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blog.Infra.Configs
{
    internal class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("tb_user");

            builder.HasKey(u => u.Id);

            builder.Property(e => e.Id)
                .HasColumnName("guid")
                .HasColumnType("uuid");

            builder.Property(u => u.Name)
                .HasColumnName("name")
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .HasColumnType("varchar")
                .IsRequired();

            builder.Property(u => u.Password)
                .HasColumnName("password")
                .HasColumnType("varchar")
                .IsRequired();

            builder.HasMany(u => u.Favorites)
                .WithMany(p => p.FavoritedBy)
                .UsingEntity<Dictionary<string, object>>    ( 
                    "tb_favorites", 
                    j => j
                        .HasOne<Post>()
                        .WithMany()
                        .HasForeignKey("post_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<User>()
                        .WithMany()
                        .HasForeignKey("user_id")
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.HasKey("post_id", "user_id"); 
                    }
                );


        }
    }
}
