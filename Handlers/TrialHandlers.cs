using Microsoft.EntityFrameworkCore;

public static class TrialHandlers
{
    public static async Task<IResult> GetTrial(AppDbContext db)
    {
        var trials = await db.Trials.Select(t => new TrialResponse
        {
            Id = t.Id,
            Appointment = t.Appointment,
            Duration = t.Duration,
            Parent = t.Parent == null ? null : new UserResponseBasic
            {
                Id = t.Parent.Id,
                Name = t.Parent.Name,
                Email = t.Parent.Email
            },
            Course = t.Course == null ? null : new Course
            {
                Id = t.Course.Id,
                Name = t.Course.Name
            }
        }
            ).ToListAsync();
        return Results.Ok(trials);
    }
    public static async Task<IResult> CreateTrial(TrialCreateRequest request, AppDbContext db)
    {
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Parent.Email);
        if (user == null)
        {
            User newUser = new User
            {
                Name = request.Parent.Name,
                Email = request.Parent.Email,
                Country = request.Parent.Country,
                Phone = request.Parent.Phone,
                Role = Role.Parent,
                RegisteredAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            try
            {
                await db.Users.AddAsync(newUser);
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
            user = newUser;
        }
        Trial newTrial = new Trial
        {
            Duration = request.Duration,
            Appointment = request.Appointment,
            ParentId = user.Id,
            CourseId = request.CourseId
        };
        var course = await db.Courses.FindAsync(request.CourseId);
        if (course == null)
        {
            return Results.BadRequest("this course is still not available yet, come back later!");
        }
        try
        {
            var existingTrial = await db.Trials.FirstOrDefaultAsync(t =>
                t.ParentId == user.Id &&
                t.CourseId == request.CourseId &&
                t.Appointment == request.Appointment);
            if (existingTrial != null)
            {
                return Results.Conflict("You have already booked a trial for this course at the same time.");
            }
        }
        catch (Exception ex)
        {
            return Results.Problem($"An unexpected error occurered: {ex.Message}");
        }
        await db.Trials.AddAsync(newTrial);
        await db.SaveChangesAsync();
        return Results.Created();
    }
}