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

            entity.Property(u => u.Id)
                .ValueGeneratedOnAdd();

            entity.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(u => u.Phone)
                .HasMaxLength(20);

            entity.Property(u => u.Email)
                .HasMaxLength(100);

            entity.Property(u => u.Country)
                .HasMaxLength(3);

            entity.Property(u => u.Role)
                .HasConversion<string>();

            entity.Property(u => u.RegisteredAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(u => u.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Trial>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Id)
                .ValueGeneratedNever();

            entity.Property(t => t.Duration)
                .HasDefaultValue(30);

            entity.Property(t => t.Appointment)
                .IsRequired();

            entity.HasOne(t => t.Parent)
                  .WithMany()
                  .HasForeignKey(t => t.ParentId)
                  .IsRequired()
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(t => t.Course)
                  .WithMany()
                  .HasForeignKey(t => t.CourseId)
                  .IsRequired();
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(128);
        });

        base.OnModelCreating(modelBuilder);
    }
}