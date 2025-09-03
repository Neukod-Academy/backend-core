using Microsoft.EntityFrameworkCore;

public static class TrialEndpoints
{
    public static void RegisterTrialEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api");
        api.MapGet("/trials", TrialHandlers.GetTrial)
            .WithName("GetTrials").WithTags("Trials");
        api.MapPost("/trials", TrialHandlers.CreateTrial)
            .WithName("TrialPost").WithTags("Trials");
    }
}