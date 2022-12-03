using Application.Profiles;
using Domain.Models;

namespace API.Dto
{
    public class GameDto
    {
        public Guid Id { get; set; }

        public string? CurrentState { get; set; }

        public ICollection<Profile> Participants { get; set; } = new List<Profile>();
    }
}
