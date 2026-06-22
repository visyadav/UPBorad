using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Students.Create;

public class SubjectMarksDto
{
    [JsonPropertyName("n")]
    public string SubjectName { get; set; } = string.Empty;

    [JsonPropertyName("a")]
    public string MaxMarksTheory { get; set; } = string.Empty;

    [JsonPropertyName("b")]
    public string MaxMarksInternal { get; set; } = string.Empty;

    [JsonPropertyName("c")]
    public string MaxMarksTotal { get; set; } = string.Empty;

    [JsonPropertyName("d")]
    public string MinMarksTheory { get; set; } = string.Empty;

    [JsonPropertyName("e")]
    public string MinMarksInternal { get; set; } = string.Empty;

    [JsonPropertyName("f")]
    public string ObtainedTheory { get; set; } = string.Empty;

    [JsonPropertyName("g")]
    public string ObtainedInternal { get; set; } = string.Empty;

    [JsonPropertyName("t")]
    public string TotalMarks { get; set; } = string.Empty;

    [JsonPropertyName("r")]
    public string Remarks { get; set; } = string.Empty;
}

public class CreateStudentsCommand : IRequest<string>
{
    [JsonPropertyName("rn")]
    public string RollNumber { get; set; } = string.Empty;

    [JsonPropertyName("cn")]
    public string CentreNo { get; set; } = string.Empty;

    [JsonPropertyName("sn")]
    public string SchoolNo { get; set; } = string.Empty;

    [JsonPropertyName("en")]
    public string EnrolmentNo { get; set; } = string.Empty;

    [JsonPropertyName("et")]
    public string ExamType { get; set; } = string.Empty;

    [JsonPropertyName("nm")]
    public string StudentName { get; set; } = string.Empty;

    [JsonPropertyName("fn")]
    public string FatherName { get; set; } = string.Empty;

    [JsonPropertyName("mn")]
    public string MotherName { get; set; } = string.Empty;

    [JsonPropertyName("db")]
    public string DateOfBirth { get; set; } = string.Empty;

    [JsonPropertyName("sc")]
    public string SchoolName { get; set; } = string.Empty;

    [JsonPropertyName("ph")]
    public string PhotoPath { get; set; } = string.Empty;

    [JsonPropertyName("sb")]
    public List<SubjectMarksDto> Subjects { get; set; } = new();

    [JsonPropertyName("gt")]
    public string GrandTotal { get; set; } = string.Empty;

    [JsonPropertyName("mx")]
    public string MaxTotal { get; set; } = string.Empty;

    [JsonPropertyName("rt")]
    public string ResultText { get; set; } = string.Empty;

    [JsonPropertyName("gw")]
    public string GrandTotalWords { get; set; } = string.Empty;

    [JsonPropertyName("gd")]
    public string Grade { get; set; } = string.Empty;

    [JsonPropertyName("ps")]
    public string PassingStatus { get; set; } = string.Empty;

    [JsonPropertyName("er")]
    public string ExtraRemarks { get; set; } = string.Empty;

    [JsonPropertyName("rd")]
    public string ResultDate { get; set; } = string.Empty;

    [JsonPropertyName("py")]
    public string PassingYear { get; set; } = string.Empty;
}