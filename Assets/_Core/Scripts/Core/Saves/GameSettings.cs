using System;

namespace Core.Saves
{
    [Serializable]
    public class GameSettings
    {
        public string resolution;
        public bool isFullScreen;
        public string language;
        public float soundVolume;
        public float musicVolume;

        public GameSettings()
        {
            resolution = "1920x1080";
            isFullScreen = true;
            language = "Русский";
            soundVolume = 0.5f;
            musicVolume = 0.5f;
        }
    }
}