using Echoes.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Echoes.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => 
            {
                entity.HasKey(u => u.UserId);
                entity.Property(u => u.UserName)
                    .IsRequired();
                entity.Property(u => u.Password)
                    .IsRequired();
                entity.Property(u => u.ActiveStatus)
                    .IsRequired();
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.PostId);
                entity.Property(p => p.Title)
                    .IsRequired();
                entity.Property(p => p.Content)
                    .IsRequired();

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Posts) // from users (ICollection<Post> Posts)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasKey(l => l.Id);

                entity.HasOne(l => l.User)
                    .WithMany(u => u.Likes) // from users (ICollection<Like> Likes)
                    .HasForeignKey(l => l.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(l => l.Post)
                    .WithMany(p => p.Likes) // from posts (ICollection<Like> Likes)
                    .HasForeignKey(l => l.PostId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(f => f.Id);

                entity.HasOne(f => f.User)
                    .WithMany(u => u.Accounts) // from users (ICollection<Friend> Accounts)
                    .HasForeignKey(f => f.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(f => f.FriendUser)
                    .WithMany(u => u.Friends) // from posts (ICollection<Friend> Friends)
                    .HasForeignKey(f => f.FriendId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.CommentId);
                entity.Property(c => c.Content)
                    .IsRequired();

                entity.HasOne(c => c.User)
                    .WithMany(u => u.Comments) // from users (ICollection<Comment> Comments)
                    .HasForeignKey(c => c.UserId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(c => c.Post)
                    .WithMany(u => u.Comments) // from posts (ICollection<Comment> Comments)
                    .HasForeignKey(c => c.PostId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
