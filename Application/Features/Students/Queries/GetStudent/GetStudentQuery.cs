using Domain.Entities;
using MediatR;

namespace Application.Features.Students.Queries.GetStudent;

public record GetStudentQuery(string RollNumber) : IRequest<StudentResult>;
