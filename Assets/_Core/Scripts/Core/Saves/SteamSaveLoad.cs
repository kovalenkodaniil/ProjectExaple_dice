using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Utils;

namespace Core.Saves
{
    public class SteamSaveLoad : ISaveLoad
    {
        private readonly string FILE_PATH = $"{Application.persistentDataPath}/Saves/Save.json";
        
        private SerializableDictionary<string, string> saves = new();
        
        public SteamSaveLoad()
        {
            string directoryPath = Path.GetDirectoryName(FILE_PATH);
            
            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            if (File.Exists(FILE_PATH))
            {
                string json = File.ReadAllText(FILE_PATH);
                saves = JsonUtility.FromJson<SerializableDictionary<string, string>>(json);
            }
        }
        
        public void Save(string key, string value) => SaveToDictionary(key, value);
        public string Load(string key) => saves.ContainsKey(key) ? saves[key] : default;
        
        private void SaveToDictionary(string key, string value)
        {
            saves[key] = value;
            string json = JsonUtility.ToJson(saves);
            File.WriteAllText(FILE_PATH, json);
        }
    }
}