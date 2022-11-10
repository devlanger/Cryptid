using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cryptid.Shared
{
    public interface IGameServer
    {
        void AskToJoinMatchmaking();
        void AskToRemoveMatchmaking();
    }
}
