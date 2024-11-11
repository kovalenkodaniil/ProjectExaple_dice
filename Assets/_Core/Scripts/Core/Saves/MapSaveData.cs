using System;
using System.Collections.Generic;

namespace Core.Saves
{
    [Serializable]
    public class MapSaveData
    {
        public string currentLocationId;
        public List<LocationData> locationData;
        public List<EncounterData> mapEventData;

        public MapSaveData()
        {
            locationData = new List<LocationData>();
            mapEventData = new List<EncounterData>();
        }
    }
}