using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Features.Students.Create;

public class CreateStudentsCommandHandler : IRequestHandler<CreateStudentsCommand, string>
{
    private readonly IApplicationDbContext _context;

    public CreateStudentsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(CreateStudentsCommand request, CancellationToken cancellationToken)
    {
        var entity = new StudentResult
        {
            RollNumber = request.RollNumber,
            CentreNo = request.CentreNo,
            SchoolNo = request.SchoolNo,
            EnrolmentNo = request.EnrolmentNo,
            ExamType = request.ExamType,
            StudentName = request.StudentName,
            FatherName = request.FatherName,
            MotherName = request.MotherName,
            DateOfBirth = request.DateOfBirth,
            SchoolName = request.SchoolName,
            PhotoPath = request.PhotoPath,
            GrandTotal = request.GrandTotal,
            MaxTotal = request.MaxTotal,
            ResultText = request.ResultText,
            GrandTotalWords = request.GrandTotalWords,
            Grade = request.Grade,
            PassingStatus = request.PassingStatus,
            ExtraRemarks = request.ExtraRemarks,
            ResultDate = request.ResultDate,
            PassingYear = request.PassingYear,
            Subjects = request.Subjects.Select(s => new SubjectMarks
            {
                StudentResultRollNumber = request.RollNumber,
                SubjectName = s.SubjectName,
                MaxMarksTheory = s.MaxMarksTheory,
                MaxMarksInternal = s.MaxMarksInternal,
                MaxMarksTotal = s.MaxMarksTotal,
                MinMarksTheory = s.MinMarksTheory,
                MinMarksInternal = s.MinMarksInternal,
                ObtainedTheory = s.ObtainedTheory,
                ObtainedInternal = s.ObtainedInternal,
                TotalMarks = s.TotalMarks,
                Remarks = s.Remarks
            }).ToList()
        };

        _context.StudentResults.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.RollNumber;
    }
}