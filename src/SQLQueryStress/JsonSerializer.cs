using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SQLQueryStress
{
    internal static class JsonSerializer
    {
        public static T ReadToObject<T>(string json)
        {
            using var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var serializer = new DataContractJsonSerializer(typeof(T));
            T retObj = (T)serializer.ReadObject(memoryStream);
            return retObj;
        }

        public static string WriteFromObject<T>(T user)
        {
            using var memoryStream = new MemoryStream();
            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(memoryStream, user);
            byte[] json = memoryStream.ToArray();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }
    }
}
