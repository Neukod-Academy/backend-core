using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Npgsql;


var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.Load();
var POSTGRES_HOST = System.Environment.GetEnvironmentVariable("POSTGRES_HOST_DOCKER");
var POSTGRES_USER = System.Environment.GetEnvironmentVariable("POSTGRES_USER");
var POSTGRES_PASS = System.Environment.GetEnvironmentVariable("POSTGRES_PASS");
var POSTGRES_DB = System.Environment.GetEnvironmentVariable("POSTGRES_DB");
var connectionString = $"Host={POSTGRES_HOST};Username={POSTGRES_USER};Password={POSTGRES_PASS};Database={POSTGRES_DB}";
Console.WriteLine(connectionString);
await using var dataSource = NpgsqlDataSource.Create(connectionString);
await using var conn = await dataSource.OpenConnectionAsync() ?? throw new Exception("failed to create database connection!");
Console.WriteLine("PostgresSQL connection established");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "NeukodCoreAPI";
    config.Title = "Neukod v0";
    config.Version = "v0";
});


builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseOpenApi();
    app.Use(async (context, next) =>
    {
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

        if (authHeader != null && authHeader.StartsWith("Basic "))
        {
            var encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            var decodedBytes = Convert.FromBase64String(encodedUsernamePassword);
            var decoded = System.Text.Encoding.UTF8.GetString(decodedBytes);
            var parts = decoded.Split(':', 2);

            var username = parts[0];
            var password = parts[1];

            if (username == "neukod" && password == "uyeuye")
            {
                await next();
                return;
            }
        }

        // Unauthorized
        context.Response.Headers["WWW-Authenticate"] = "Basic realm=\"Swagger UI\"";
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
    });
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "NeukodCoreAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}


app.MapGet("/", () =>
{
    return "hello from neukod backend core!";
}).WithName("HelloApi");


app.MapGet("/trials", async (AppDbContext db)=>
{
    var trials = await db.Trials
        .Include(t => t.Parent)
        .Include(t => t.Course)
        .ToListAsync();
    return Results.Ok(trials);
}
);

app.MapPost("/trials", async (TrialCreateRequest request, AppDbContext db) =>
{
    var course = await db.Courses.FindAsync(request.CourseId);
    if (course == null)
    {
        return Results.BadRequest("this course is still not available yet, come back later!");
    }
    var user = await db.Users.FirstOrDefaultAsync(u => u.Email == request.Parent.Email);
    if (user == null)
    {
        return Results.BadRequest("this parent account have not registered yet");
    }
    return Results.Created();
}
).WithName("TrialPost");

app.Run();