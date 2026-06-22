using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Domain.Entities;

public class SubjectMarks
{
    [Key]
    public int Id { get; set; }

    public string StudentResultRollNumber { get; set; } = string.Empty; // Foreign Key

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