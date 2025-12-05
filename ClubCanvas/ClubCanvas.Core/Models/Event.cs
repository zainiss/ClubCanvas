using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClubCanvas.Core.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Location { get; set; }
        public int ClubId { get; set; }
        public Club Club {get; set;}
        public List<ApplicationUser> Attendees { get; set; } = new List<ApplicationUser>();

        public void AddAttendee(ApplicationUser user)
        {
            Attendees.Add(user);
        }

        public void RemoveAttendee(ApplicationUser user)
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

