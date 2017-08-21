using Newtonsoft.Json;
using System;
using System.IO;

namespace INVO.Core
{
    public class Configuration
    {
        [JsonIgnore]
        /// <summary> The location of Cores configuration file. </summary>
        public static string FileName { get; private set; } = "config/configuration.json";
        /// <summary> User IDs of people who can remove Cores batteries. </summary>
        public ulong[] Owners { get; set; }
        /// <summary> Cores command prefix. </summary>
        public string Prefix { get; set; } = ".";
        /// <summary> Cores login token. </summary>
        public string Token { get; set; } = "";

        public static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            if (!File.Exists(file))                                 // Check if the configuration file exists.
            {
                string path = Path.GetDirectoryName(file);          // Create config directory if doesn't exist.
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var config = new Configuration();                   // Create a new configuration object.

                Console.WriteLine("Please enter your token: ");
                string token = Console.ReadLine();                  // Read the bot token from console, first time setup.

                config.Token = token;
                config.SaveJson();                                  // Save the new configuration object to file, will now need to edit through file explorer.
            }
            Console.WriteLine("Configuration Loaded");
            Console.WriteLine(file.ToString());
        }

        /// <summary> Save the configuration to the path specified in FileName. </summary>
        public void SaveJson()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllText(file, ToJson());
        }

        /// <summary> Load the configuration from the path specified in FileName. </summary>
        public static Configuration Load()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(file));
        }

        /// <summary> Convert the configuration to a json string. </summary>
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);
    }
}