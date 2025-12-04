using System.Collections.Generic;
using System.Threading.Tasks;
using ClubCanvas.Core.Models;

namespace ClubCanvas.Core
{
    public interface IClubsRepository
    {
        Task<List<Club>> GetAllClubsAsync();
        Task<Club?> GetClubByIdAsync(int id);
        Task AddClubAsync(Club club);
        Task UpdateClubAsync(Club club);
        Task DeleteClubAsync(int id);
    }
}