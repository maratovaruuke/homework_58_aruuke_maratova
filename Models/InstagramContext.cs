using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace homework_59_aruuke_maratova.Models
{
    public class InstagramContext : IdentityDbContext<User>
    {
        public DbSet<User> ContextUsers { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follow> Follows { get; set; }

        public InstagramContext(DbContextOptions<InstagramContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Follow>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd();

            builder.Entity<Follow>()
                .HasOne(l => l.Followee)
                .WithMany(a => a.Followers)
                .HasForeignKey(l => l.FolloweeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Follow>()
                .HasOne(l => l.Follower)
                .WithMany(a => a.Following)
                .HasForeignKey(l => l.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}
