using Domain.Entities;
using Microsoft.EntityFrameworkCore;

using Application.Interfaces;

namespace Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<StudentResult> StudentResults { get; set; }
    public DbSet<SubjectMarks> SubjectMarks { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configure StudentResult
        modelBuilder.Entity<StudentResult>(entity =>
        {
            // Limit the length of the Primary Key so it can be indexed properly
            entity.Property(e => e.RollNumber).HasMaxLength(50);
            
            // Configure the One-to-Many relationship with SubjectMarks
            entity.HasMany(e => e.Subjects)
                  .WithOne() // WithOne is empty because SubjectMarks doesn't have a navigation property back to StudentResult
                  .HasForeignKey(sm => sm.StudentResultRollNumber)
                  .OnDelete(DeleteBehavior.Cascade); // Automatically delete SubjectMarks if the StudentResult is deleted
        });

        // 2. Configure SubjectMarks
        modelBuilder.Entity<SubjectMarks>(entity =>
        {
            // The foreign key also needs a max length to match the primary key it points to
            entity.Property(e => e.StudentResultRollNumber).HasMaxLength(50);
        });
    }
    
}