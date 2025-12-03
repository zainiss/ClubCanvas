using System.Collections.Generic;
using System.Linq;

namespace ClubCanvas.Core.Models;

public class Club
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Event> Events { get; set; }
    public List<User> Members { get; set; }
    public User Owner { get; set; }
    public string Image { get; set; }

    public void AddMember(User member)
    {
        if (member == null)
        {
            return;
        }

        if (Members == null)
        {
            Members = new List<User>();
        }

        // Check if member already exists
        if (!Members.Any(m => m.Id == member.Id))
        {
            Members.Add(member);
        }
    }

    public void RemoveMember(User member)
    {
        if (member == null || Members == null)
        {
            return;
        }

        var memberToRemove = Members.FirstOrDefault(m => m.Id == member.Id);
        if (memberToRemove != null)
        {
            Members.Remove(memberToRemove);
        }
    }

    public void AddEvent(Event eventToAdd)
    {
        if (eventToAdd == null)
        {
            return;
        }

        if (Events == null)
        {
            Events = new List<Event>();
        }

        // Check if event already exists
        if (!Events.Any(e => e.Id == eventToAdd.Id))
        {
            Events.Add(eventToAdd);
        }
    }

    public void RemoveEvent(Event eventToRemove)
    {
        if (eventToRemove == null || Events == null)
        {
            return;
        }

        var eventToDelete = Events.FirstOrDefault(e => e.Id == eventToRemove.Id);
        if (eventToDelete != null)
        {
            Events.Remove(eventToDelete);
        }
    }
}

