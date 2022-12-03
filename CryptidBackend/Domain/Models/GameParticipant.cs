using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class GameParticipant
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public Guid GameId { get; set; }
        public Game Game { get; set; }
    }
}
