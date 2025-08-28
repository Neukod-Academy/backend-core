public class Course
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Trial> Trials { get; set; } = new List<Trial>();
}