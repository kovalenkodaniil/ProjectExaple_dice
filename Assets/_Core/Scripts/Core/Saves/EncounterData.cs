using System;

namespace Core.Saves
{
    [Serializable]
    public class EncounterData
    {
        public string id;
        public bool isComplete;
        public bool isUnlock;

        public EncounterData()
        {
            isComplete = false;
            isUnlock = false;
        }
    }
}