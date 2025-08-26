using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using Npgsql;


DotNetEnv.Env.Load();
var POSTGRES_HOST = System.Environment.GetEnvironmentVariable("POSTGRES_HOST_DOCKER");
var POSTGRES_USER = System.Environment.GetEnvironmentVariable("POSTGRES_USER");
var POSTGRES_PASS = System.Environment.GetEnvironmentVariable("POSTGRES_PASS");
var POSTGRES_DB = System.Environment.GetEnvironmentVariable("POSTGRES_DB");
var connectionString = $"Host={POSTGRES_HOST};Username={POSTGRES_USER};Password={POSTGRES_PASS};Database={POSTGRES_DB}";
await using var dataSource = NpgsqlDataSource.Create(connectionString);
await using var conn = await dataSource.OpenConnectionAsync() ?? throw new Exception("failed to create database connection!");
Console.WriteLine("PostgresSQL connection established");

var builder = WebApplication.CreateBuilder(args);

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


app.MapGet("/trials", ()=>
{
    var repo = new TrialRepository(connectionString);
    return repo.GetTrialsAsync();
});

app.MapPost("/trials", (Trial NewTrial) =>
    {
        Console.WriteLine("new trial: "+NewTrial);
        return Results.Created("/trials",NewTrial);
    }
).WithName("TrialPost");

app.Run();