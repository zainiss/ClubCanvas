using System.Collections.Generic;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Core
{
    public interface IEventsRepository
    {
        List<Event> GetAllEvents();
        Event GetEventById(int id);
        void AddEvent(Event e);
        void UpdateEvent(Event e);
        void DeleteEvent(int id);
    }
}