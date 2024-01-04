using WebApp.Concerts.Models;

namespace WebApp.Concerts;

public interface IConcertService
{
    Task<Concert> CreateConcertAsync(NewConcert newConcert);
    Task DeleteConcertAsync(int id);
    Task<Concert> EditConcertAsync(Concert member);
    Task<Concert> GetConcertByIdAsync(int id);
    Task<List<Concert>> GetConcertsAsync();
}
