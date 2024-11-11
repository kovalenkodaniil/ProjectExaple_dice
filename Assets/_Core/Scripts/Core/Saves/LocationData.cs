using System;
using System.Collections.Generic;

namespace Core.Saves
{
    [Serializable]
    public class LocationData
    {
        public string id;
        public bool isUnlock;
        public bool isComplete;
        public List<EncounterData> encountersData;

        public LocationData()
        {
            encountersData = new List<EncounterData>();

            isComplete = false;
            isUnlock = false;
        }
    }
}