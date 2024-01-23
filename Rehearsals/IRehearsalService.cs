using WebApp.Rehearsals.Models;

namespace WebApp.Rehearsals;

public interface IRehearsalsService
{
    Task<Rehearsal> CreateRehearsalAsync(CreateRehearsalModel newRehearsal);
    Task<Rehearsal?> DeleteRehearsalAsync(int id);
    Task<Rehearsal?> EditRehearsalAsync(Rehearsal member);
    Task<Rehearsal?> GetRehearsalByIdAsync(int id);
    Task<List<Rehearsal>> GetRehearsalsAsync();
}
