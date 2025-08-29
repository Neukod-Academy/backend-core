using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
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
    var newTrial = new Trial
    {
        Appointment = request.Appointment,
        Parent = request.Parent,
        Course = request.Course,
    };
    await db.Trials.AddAsync(newTrial);
    await db.SaveChangesAsync();
    return Results.Created($"/trials/{newTrial.Id}", newTrial);
}
).WithName("TrialPost");

app.Run();