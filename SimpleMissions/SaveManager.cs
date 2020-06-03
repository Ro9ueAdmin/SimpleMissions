using System;
using System.Collections.Generic;
using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleMissions
{
    internal class SaveManager
    {
        public static Save save;
        private static string saveFile = Main.Saves + $"\\{Environment.MachineName}.sms";
        private static BinaryFormatter formatter = new BinaryFormatter();

        /// <summary>
        /// Loads the save from AppData
        /// </summary>
        public static void LoadSave()
        {
            if(File.Exists(saveFile))
            {
                Stream saveStream = new FileStream(saveFile, FileMode.Open);
                save = (Save)formatter.Deserialize(saveStream);
                saveStream.Close();
            }
            else
            {
                save = new Save()
                {
                    completedMissions = new List<string>()
                };
                Save();
            }
        }

        /// <summary>
        /// Saves the current save to AppData
        /// </summary>
        public static void Save()
        {
            Stream saveStream = new FileStream(saveFile, FileMode.Create);
            formatter.Serialize(saveStream, save);
            saveStream.Close();
        }
    }

    [Serializable]
    internal class Save
    {
        // Note to future self: ADD MORE HERE!! You've got to be forgetting *something*!
        public List<string> completedMissions { get; set; }
    }
}
