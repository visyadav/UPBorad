using Application.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Students.Queries.GetStudent;

public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, StudentResult?>
{
    private readonly IApplicationDbContext _context;

    public GetStudentQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<StudentResult?> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentResults
            .Include(s => s.Subjects)
            .FirstOrDefaultAsync(s => s.RollNumber == request.RollNumber, cancellationToken);
    }
}
