using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace CryptidClient.Assets.Scripts.MapLoader
{
    public class GameStateSerializationHelper
    {
        public static void Save (string prefKey, object serializableObject)
        {
            MemoryStream memoryStream = new MemoryStream ();
            new BinaryFormatter ().Serialize (memoryStream, serializableObject);
            string tmp = System.Convert.ToBase64String (memoryStream.ToArray ());
        }
    
        public static T Load<T>(string prefKey)
        {
            string serializedData = "";
            MemoryStream dataStream = new MemoryStream(System.Convert.FromBase64String(serializedData));
        
            T deserializedObject = (T)new BinaryFormatter().Deserialize(dataStream);
        
            return deserializedObject;
        }
    }
}