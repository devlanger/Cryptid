using System;
using System.Threading.Tasks;

namespace Cryptid.Shared
{
    public interface IGameClient
    {
        Task SendMessage(string messageName, string data);
        Task EnterGame(string gameName);
        Task ChangeMenuState(byte state);
        Task HandleActionCommand(string commandJson);
    }
}
