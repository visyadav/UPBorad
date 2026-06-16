using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<StudentResult> StudentResults { get; set; }
    public DbSet<SubjectMarks> SubjectMarks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<StudentResult>()
            .HasMany(s => s.Subjects)
            .WithOne()
            .HasForeignKey(sm => sm.StudentResultRollNumber);
    }
}
