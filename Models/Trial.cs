using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

public class Trial
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Duration { get; set; } = 30;
    public DateTime Appointment { get; set; }
    public User Parent { get; set; }
    public Course Course { get; set; }
}