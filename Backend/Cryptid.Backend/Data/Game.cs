using Backend.Data;
using Cryptid.Backend.Data;

namespace Backend.Logic
{
    public class Game : IGame
    {
        public Guid Id { get; set; }

        public byte[]? CurrentState { get; set; }
    }

    public interface IGame
    {
        Guid Id { get; set; }

        byte[] CurrentState { get; set; }
    }
}