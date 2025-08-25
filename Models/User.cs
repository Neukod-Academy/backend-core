
using System.Text.Json.Serialization;

class User
{
    [JsonPropertyName("name")]
    public String? Name { get; set; }
    [JsonPropertyName("role")]
    public Role Role { get; set; }
}


enum Role
{
    Visitor,
    Parent,
    Student,
    Teacher,
}