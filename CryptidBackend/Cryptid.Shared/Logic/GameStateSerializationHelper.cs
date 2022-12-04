using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CryptidClient.Assets.Scripts.MapLoader
{
    public class GameStateSerializationHelper
    {
        public static string Save (GameState serializableObject)
        {
            // MemoryStream memoryStream = new MemoryStream ();
            // new BinaryFormatter ().Serialize (memoryStream, serializableObject);
            // string tmp = System.Convert.ToBase64String (memoryStream.ToArray ());
            // return tmp;

            return JsonConvert.SerializeObject(serializableObject);
        }
    
        public static T Load<T>(string serializedData)
        {
            return JsonConvert.DeserializeObject<T>(serializedData);

            // MemoryStream dataStream = new MemoryStream(System.Convert.FromBase64String(serializedData));
        
            // T deserializedObject = (T)new BinaryFormatter().Deserialize(dataStream);
        
            // return deserializedObject;
        }
    }
}