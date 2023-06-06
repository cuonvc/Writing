using Microsoft.EntityFrameworkCore;
using Writing.Entities;
using Writing.Enumerates;

namespace Writing.Repositories; 

public class DataContext : DbContext {

    public DataContext(DbContextOptions<DataContext> options) : base(options) {
        
    }

    protected DataContext() {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Relationship> Relationships { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserPostVote> UserPostVotes { get; set; }

    // protected override void OnModelCreating(ModelBuilder modelBuilder) {
    //     base.OnModelCreating(modelBuilder);
    //     
    //     modelBuilder.Entity<UserPostVote>()
    //         .HasKey(vote => new {vote.UserId, vote.PostId});
    //
    //     modelBuilder.Entity<UserPostVote>()
    //         .HasOne(vote => vote.User)
    //         .WithMany()
    //         .HasForeignKey(vote => vote.UserId);
    //
    //     modelBuilder.Entity<UserPostVote>()
    //         .HasOne(vote => vote.Post)
    //         .WithMany()
    //         .HasForeignKey(vote => vote.PostId);
    // }
}