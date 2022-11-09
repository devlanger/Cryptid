namespace Backend.Logic
{
    public class Game : IGame
    {
        public Guid Id { get; set; }
    }

    public interface IGame
    {
        Guid Id { get; set; }
    }
}