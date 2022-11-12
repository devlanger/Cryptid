using System.Runtime.Serialization.Formatters.Binary;

namespace Cryptid.Backend.Data
{
    public class GameState
    {
        public string GameName { get; set; } = "TestGame";

        public byte[] GetCompressedState()
        {
            //var obj = this;
            //
            //BinaryFormatter bf = new BinaryFormatter();
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    bf.Serialize(ms, obj);
            //    return ms.ToArray();
            //}

            return new byte[1];
        }
    }
}
