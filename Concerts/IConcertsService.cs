using WebApp.Concerts.Models;

namespace WebApp.Concerts;

public interface IConcertsService
{
    Task<Concert> CreateConcertAsync(CreateConcertModel newConcert);
    Task DeleteConcertAsync(int id);
    Task<Concert> EditConcertAsync(Concert member);
    Task<Concert> GetConcertByIdAsync(int id);
    Task<List<Concert>> GetConcertsAsync();
}
