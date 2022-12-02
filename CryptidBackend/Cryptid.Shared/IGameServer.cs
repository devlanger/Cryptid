using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cryptid.Shared
{
    public interface IGameServer
    {
        Task AskToJoinMatchmaking();
        Task AskToRemoveMatchmaking();
        Task LoginWithAccessToken(string userId, string token);
    }
}
