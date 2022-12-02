namespace Domain.Models
{
    public class Game : IGame
    {
        public Guid Id { get; set; }

        public byte[]? CurrentState { get; set; }
    }

    public interface IGame
    {
        Guid Id { get; set; }

        // byte[] CurrentState { get; set; }
    }
}