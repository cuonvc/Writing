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

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        // modelBuilder.Entity<Comment>()
        //     .HasOne<User>(s => s.User)
        //     .WithMany(c => c.Comments)
        //     .HasForeignKey(u => u.)
    }
    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer("server=MIELODIE\\SQLEXPRESS; database = project_C#; integrated security=sspi;encrypt=true;trustservercertificate=true;");
    // }
}