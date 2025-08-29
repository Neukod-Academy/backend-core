using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class TrialCreateRequest
{
    public DateTime Appointment { get; set; }
    public required ParentInfo Parent { get; set; }
    [JsonPropertyName("course_id")]
    public required int CourseId { get; set; }
    [Range(30, 60, ErrorMessage = "Duration must be between 1 and 480 minutes.")]
    public required int Duration { get; set; }
}

public class ParentInfo
{
    [Required]
    public required string Name { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
    [Phone]
    public string? Phone { get; set; }

    public string? Country { get; set; }
}