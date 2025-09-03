using Microsoft.EntityFrameworkCore;

public static class CourseEndpoints
{
    public static void RegisterCourseEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");
        api.MapPost("/courses", CourseHandlers.RegisterCourse)
            .WithName("CoursePost").WithTags("Courses");
        api.MapGet("/courses", CourseHandlers.GetCourse)
            .WithName("CoursesGet").WithTags("Courses");
    }
}