using MiniProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniProject.Repositories
{
    internal class Repository
    {
        //public static readonly string _path = @"C:\Users\ertug\Desktop\MiniProject\MiniProject\Files\Orders.json";


        public static void Serialize<T>(string filePath, List<T> items)
        {
            string json = JsonConvert.SerializeObject(items);

            using(StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(json);
            }
        }

        public static List<T> Deserialize<T>(string  filePath)
        {
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            string json;

            using (StreamReader sr = new StreamReader(filePath))
            {
                json = sr.ReadToEnd();
            }

            return JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
        }
    }
}