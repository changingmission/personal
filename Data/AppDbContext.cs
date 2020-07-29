using Microsoft.EntityFrameworkCore;
using Personal.Entities;

namespace Personal.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<PostCategory> PostCategories { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    protected override void OnModelCreating(ModelBuilder mb)
    {
      base.OnModelCreating(mb);

      mb.Entity<PostTag>()
        .HasKey(pt => new { pt.PostId, pt.TagId });

      mb.Entity<PostCategory>()
        .HasKey(pc => new { pc.PostId, pc.CategoryId });

      mb.Entity<Post>()
        .Property(p=>p.Title).IsRequired();
      mb.Entity<Post>()
        .Property(p=>p.Slug).IsRequired();
      mb.Entity<Tag>()
        .Property(p=>p.Title).IsRequired().HasMaxLength(255);
      mb.Entity<Tag>()
        .Property(p=>p.Slug).IsRequired();
      // mb.Entity<Tag>().HasIndex(t=>t.Slug).IsUnique();
      mb.Entity<Category>()
        .Property(p=>p.Title).IsRequired().HasMaxLength(255);
      mb.Entity<Category>()
        .Property(p=>p.Slug).IsRequired();
      
    }

  }
}