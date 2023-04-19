using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtnennaSwitcher
{
   static class ConfigProvider
    {
        public static void DeserializeConfiguration(Configuration objectToWrite, bool append = false, bool reset = false)
        {
            TextWriter writer = null;
            try
            {

                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string specificFolder = Path.Combine(folder, "AntSwitcher");
                Directory.CreateDirectory(specificFolder);
                var file = specificFolder + "\\config.json";
                string contentsToWriteToFile;
                if (reset)
                {
                    var defaultConfiguration = new Configuration();



                    contentsToWriteToFile = JsonConvert.SerializeObject(defaultConfiguration);
                }
                else
                {
                    contentsToWriteToFile = JsonConvert.SerializeObject(objectToWrite);
                }

                writer = new StreamWriter(file, append);
                writer.Write(contentsToWriteToFile);
            }
            finally
            {
                writer?.Close();
            }
        }

        public static Configuration GetConfiguration()
        {
            TextReader reader = null;
            try
            {
                string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string specificFolder = Path.Combine(folder, "AntSwitcher");
                var file = specificFolder + "\\config.json";
                if (!File.Exists(file))
                {
                    var defaultConfiguration = new Configuration();

                    DeserializeConfiguration(defaultConfiguration);
                }

                reader = new StreamReader(file);
                var fileContents = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<Configuration>(fileContents);
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
