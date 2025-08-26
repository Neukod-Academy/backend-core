using Npgsql;

public class TrialRepository : ITrialRepository
{
    private readonly string _connectionString;

    public TrialRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Trial>> GetTrialsAsync()
    {
        var trials = new List<Trial>();

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM classes", conn);
        await using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var trial = new Trial
            {
                Id = reader.GetString(reader.GetOrdinal("id")),
                Appointment = reader.IsDBNull(reader.GetOrdinal("appointment")) ? null : reader.GetDateTime(reader.GetOrdinal("appointment")),
                Duration = reader.IsDBNull(reader.GetOrdinal("duration")) ? 30 : reader.GetInt32(reader.GetOrdinal("duration")),
                Course = reader.IsDBNull(reader.GetOrdinal("course")) ? null : reader.GetString(reader.GetOrdinal("course")),
                Parent = null,
            };
            trials.Add(trial);
        }
        Console.WriteLine("finished fetching trials from db");
        return trials;
    }
}