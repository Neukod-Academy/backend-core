public interface ITrialRepository
{
    Task<List<Trial>> GetTrialsAsync();
}
