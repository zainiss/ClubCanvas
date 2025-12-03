using System.Collections.Generic;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Core
{
    public interface IClubsRepository
    {
        List<Club> GetAllClubs();
        Club GetClubById(int id);
        void AddClub(Club club);
        void UpdateClub(Club club);
        void DeleteClub(int id);
    }
}