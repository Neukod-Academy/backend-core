using Microsoft.EntityFrameworkCore;
using Sprache;

public static class CourseHandlers
{
    public static async Task<IResult> RegisterCourse(CourseCreateRequest request, AppDbContext db)
    {
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.BadRequest("Course name is required.");
            }
            try
            {
                Course newCourse = new Course
                {
                    Name = request.Name
                };
                await db.Courses.AddAsync(newCourse);
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                return Results.BadRequest($"Database error: {ex.Message}");
            }
            catch (Exception ex)
            {
                return Results.Problem($"An unexpected error occurered: {ex.Message}");
            }
            return Results.Created();
        }
    }

    public static async Task<IResult> GetCourse(AppDbContext db)
    {
        var courses = await db.Courses.Select(c => new CourseResponse
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync();
        return Results.Ok(courses);
    }
}