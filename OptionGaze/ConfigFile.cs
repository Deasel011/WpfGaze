//  ==========================================================================
//   Code created by Philippe Deslongchamps.
//   For the Stockgaze project.
//  ==========================================================================

using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OptionGaze
{

    public abstract class ConfigFile
    {

        private readonly string m_filename = $"{nameof(ConfigFile)}.json";

        public bool FileExist => Directory.Exists(Path) && File.Exists(System.IO.Path.Combine(Path, m_filename));

        private string Path => System.IO.Path.Combine(Environment.CurrentDirectory, "ConfigFiles");

        public Task Save()
        {
            try
            {
                if (!Directory.Exists(Path)) Directory.CreateDirectory(Path);

                File.WriteAllText(System.IO.Path.Combine(Path, m_filename), JsonConvert.SerializeObject(this));
            }
            catch (Exception e)
            {
                throw new Exception($"An exception occured while saving the configuration file [{System.IO.Path.Combine(Path, m_filename)}]");
            }

            return Task.CompletedTask;
        }

        public Task Load()
        {
            try
            {
                var serializedConfig = File.ReadAllText(System.IO.Path.Combine(Path, m_filename));

                JsonConvert.PopulateObject(serializedConfig, this);
            }
            catch (Exception e)
            {
                // Configuration defaulted
            }

            return Task.CompletedTask;
        }

    }

}