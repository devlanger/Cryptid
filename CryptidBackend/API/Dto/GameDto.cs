using Domain.Models;

namespace API.Dto
{
    public class GameDto
    {
        public Guid Id { get; set; }

        public string? CurrentState { get; set; }

        public ICollection<AppUser> Participants { get; set; }

    }
}
