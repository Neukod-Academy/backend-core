using Microsoft.EntityFrameworkCore;

public static class CourseEndpoints
{
    public static void RegisterCourseEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");
        api.MapPost("/courses", async (CourseCreateRequest request, AppDbContext db) =>
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
        }).WithName("CoursePost").WithTags("Courses");
    api.MapGet("/courses", async (AppDbContext db) =>
    {
        var courses = await db.Courses.Select(c => new CourseResponse
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync();
        return Results.Ok(courses);
    }).WithName("CoursesGet").WithTags("Courses");
    }
}