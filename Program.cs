using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
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

app.UseHttpsRedirection();


app.MapGet("/", () =>
{
    return "hello from neukod backend core!";
}).WithName("HelloApi");


app.MapPost("/trial", (Trial NewTrial) =>
    {
        Console.WriteLine("new trial: "+NewTrial);
        return Results.Created("/trial",NewTrial);
    }
).WithName("TrialPost");

app.Run();