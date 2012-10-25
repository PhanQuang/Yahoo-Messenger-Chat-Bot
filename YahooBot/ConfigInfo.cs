using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;

namespace YahooBot
{
    public class ConfigInfo
    {
        public string Username;
        public string Password { get; set; }
        public string ConsummerKey { get; set; }
        public string ConsummerSecret { get; set; }
        public string Status { get; set; }

        public ConfigInfo()
        {
            ReadConfigFile();
        }

        public void ReadConfigFile()
        {
            StreamReader stream = File.OpenText("config.dat");
            string input = stream.ReadToEnd();
            stream.Close();
            Serialize(input);
        }

        public void Serialize(string input)
        {
            JObject o = JObject.Parse(input);
            Username = o["ID"].ToString();
            Password = o["Password"].ToString();
            ConsummerKey = o["ConsumerKey"].ToString();
            ConsummerSecret = o["ConsumerSecret"].ToString();
            Status = o["Status"].ToString();
        }
    }
}
