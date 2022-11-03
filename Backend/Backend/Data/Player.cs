using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Backend.Data
{
    public class Player
    {
        [Key]
        public Guid Id { get; set; }
    }
}
