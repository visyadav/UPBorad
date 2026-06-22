using FluentValidation;

namespace Application.Features.Students.Create;

public class CreateStudentsCommandValidator : AbstractValidator<CreateStudentsCommand>
{
    public CreateStudentsCommandValidator()
    {
        RuleFor(v => v.RollNumber).NotNull().NotEmpty();
        RuleFor(v => v.CentreNo).NotNull();
        RuleFor(v => v.SchoolNo).NotNull();
        RuleFor(v => v.EnrolmentNo).NotNull();
        RuleFor(v => v.ExamType).NotNull();
        RuleFor(v => v.StudentName).NotNull();
        RuleFor(v => v.FatherName).NotNull();
        RuleFor(v => v.MotherName).NotNull();
        RuleFor(v => v.DateOfBirth).NotNull();
        RuleFor(v => v.SchoolName).NotNull();
        RuleFor(v => v.GrandTotal).NotNull();
        RuleFor(v => v.MaxTotal).NotNull();
        RuleFor(v => v.ResultText).NotNull();
    }
}