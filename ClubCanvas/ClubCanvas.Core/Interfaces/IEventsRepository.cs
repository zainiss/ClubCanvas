using System.Collections.Generic;
using System.Threading.Tasks;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Core
{
    public interface IEventsRepository
    {
        Task<List<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
        Task AddEventAsync(Event e);
        Task UpdateEventAsync(Event e);
        Task DeleteEventAsync(int id);
    }
}