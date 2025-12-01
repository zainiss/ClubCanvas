using System.ComponentModel.DataAnnotations;

namespace ClubCanvas.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        
        void ValidPassword()
        {
            
        }
        
        void ValidEmail()
        {
            
        }
    }
}
