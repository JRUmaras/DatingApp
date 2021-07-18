using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        { }

        public DbSet<AppUser> Users { get; set; }

        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureLikesEntity(modelBuilder);
        }

        private static void ConfigureLikesEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLike>()
                .HasKey(userLike => new { SourceUserId = userLike.LikerId, DestinationUserId = userLike.LikeeId });

            // Liked-users relationship
            modelBuilder.Entity<UserLike>()
                .HasOne(userLike => userLike.Liker)
                .WithMany(user => user.LikedUsers)
                .HasForeignKey(userLike => userLike.LikerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Liked-by-users relationship
            modelBuilder.Entity<UserLike>()
                .HasOne(userLike => userLike.Likee)
                .WithMany(user => user.LikedByUsers)
                .HasForeignKey(userLike => userLike.LikeeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
