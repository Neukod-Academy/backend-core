public class TrialResponse
{
    public Guid Id { get; set; }
    public required int Duration { get; set; }
    public DateTime Appointment { get; set; }
    public UserResponseBasic? Parent { get; set; }
    public Course? Course { get; set; }
}