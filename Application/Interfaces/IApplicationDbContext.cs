using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<StudentResult> StudentResults { get; }
    DbSet<SubjectMarks> SubjectMarks { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
