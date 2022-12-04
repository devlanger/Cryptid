using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cryptid.Shared
{
    public interface ISerializable
    {
        void Deserialize(BinaryReader reader);
        void Serialize(BinaryWriter writer);
    }
}
