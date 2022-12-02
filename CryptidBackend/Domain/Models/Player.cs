using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Player
    {
        public Guid Id { get; set; }

        public string AuthenticationId { get; set; }

        public string Nickname { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public DateTime RegisterTime { get; set; } = DateTime.Now;
        public DateTime LastLoginTime { get; set; } = DateTime.Now;
    }
}
