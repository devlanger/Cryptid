using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Backend.Logic;

namespace Backend.Data
{
    public class Player
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string AuthenticationId { get; set; }

        public string Nickname { get; set; } = String.Empty;
        public string Role { get; set; } = string.Empty;

        public DateTime RegisterTime { get; set; } = DateTime.Now;
        public DateTime LastLoginTime { get; set; } = DateTime.Now;
    }
}
