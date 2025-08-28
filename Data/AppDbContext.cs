using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Trial> Trials { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired();
            entity.Property(u => u.RegisteredAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(u => u.UpdatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasMany(u => u.Trials)
                  .WithOne(t => t.Parent)
                  .HasForeignKey("ParentId")
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Trial>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Id)
                .ValueGeneratedNever(); // use C# GUID
            entity.Property(t => t.Duration)
                .HasDefaultValue(30);
            entity.Property(t => t.Appointment)
                .IsRequired();

            entity.HasOne(t => t.Parent)
                  .WithMany(u => u.Trials)
                  .HasForeignKey("ParentId")
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.Course)
                  .WithMany(c => c.Trials)
                  .HasForeignKey("CourseId");
        });

        base.OnModelCreating(modelBuilder);
    }
}