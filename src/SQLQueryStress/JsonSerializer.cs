using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SQLQueryStress
{
    internal class JsonSerializer
    {
        public static T ReadToObject<T>(string json)
        {
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                T retObj = (T)serializer.ReadObject(memoryStream);
                return retObj;
            }
        }

        public static string WriteFromObject<T>(T user)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(memoryStream, user);
                byte[] json = memoryStream.ToArray();
                return Encoding.UTF8.GetString(json, 0, json.Length);
            }
        }



    }
}
