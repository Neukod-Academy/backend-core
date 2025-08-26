using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class Trial
{
    public required string Id { get; set; }
    public int Duration { get; set; } = 30;
    public required DateTime? Appointment { get; set; }
    public required User? Parent { get; set; }
    public required string? Course { get; set; }

}