using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubCanvas.mvc.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public string? Location { get; set; }
        public List<User> Attendees { get; set; }

        public void AddAttendee(User user)
        {
            Attendees.Add(user);
        }

        public void RemoveAttendee(User user)
        {
            foreach (var checkUser in Attendees)
            {
                if (checkUser.Email == user.Email)
                {
                    Attendees.Remove(user);
                }
            }
        }
        
        public void NotifyAttendees()
        {
            // add functionality
        }
    }
}