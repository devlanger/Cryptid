namespace Domain.Models
{
    public class Game : IGame
    {
        public Guid Id { get; set; }

        public string? CurrentState { get; set; }
        public ICollection<GameParticipant> Participants { get; set; } = new List<GameParticipant>();
    }

    public interface IGame
    {
        Guid Id { get; set; }

        // byte[] CurrentState { get; set; }
    }
}