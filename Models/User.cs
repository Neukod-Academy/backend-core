
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class User
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public Role Role { get; set; } = Role.Parent;
    public string? Country { get; set; }
    public DateTime RegisteredAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<Trial> Trials { get; set; } = new List<Trial>();
}


public enum Role
{
    Visitor,
    Parent,
    Student,
    Teacher,
}