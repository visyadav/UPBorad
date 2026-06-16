using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models;

public class StudentResult
{
    [Key]
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
    public List<SubjectMarks> Subjects { get; set; } = new();

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
