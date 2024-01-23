using WebApp.Rehearsals.Models;

namespace WebApp.Rehearsals;

public interface IRehearsalService
{
    Task<Rehearsal> CreateRehearsalAsync(CreateRehearsalModel newRehearsal);
    Task DeleteRehearsalAsync(int id);
    Task<Rehearsal> EditRehearsalAsync(Rehearsal member);
    Task<Rehearsal> GetRehearsalByIdAsync(int id);
    Task<List<Rehearsal>> GetRehearsalsAsync();
}
