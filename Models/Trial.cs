using System.ComponentModel;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

class Trial
{
    public int Duration { get; set; } = 30;
    [JsonPropertyName("appointment")]
    public DateTime? Appointment { get; set; }
    [JsonPropertyName("country")]
    public String? Country { get; set; }
    [JsonPropertyName("parent")]
    public User? Parent { get; set; }
    [JsonPropertyName("course")]
    public String? Course { get; set; }

}