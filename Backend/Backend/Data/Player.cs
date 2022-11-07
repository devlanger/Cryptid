using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Data
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; }

        public string Nickname { get; set; } = String.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
