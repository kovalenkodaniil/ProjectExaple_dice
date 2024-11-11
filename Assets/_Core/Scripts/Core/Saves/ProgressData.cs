using System;
using System.Collections.Generic;

namespace Core.Saves
{
    [Serializable]
    public class ChapterData
    {
        public string id;
        public bool isUnlock;
        public bool isComplete;
        public List<EncounterData> encounters;

        public ChapterData()
        {
            isComplete = false;
            isUnlock = false;
            
            encounters = new List<EncounterData>();
        }
        
        public EncounterData GetEncounterData(string searchingId)
        {
            EncounterData encounter = encounters.Find(data => data.id == searchingId);

            if (encounter == null)
            {
                encounter = new EncounterData();
                encounter.id = searchingId;
                
                encounters.Add(encounter);
            }

            return encounter;
        }
    }

    [Serializable]
    public class ProgressData
    {
        public List<ChapterData> chapters;
        public List<EncounterData> events;
        public List<EncounterData> optionalBattles;

        public ProgressData()
        {
            chapters = new List<ChapterData>();
            events = new List<EncounterData>();
            optionalBattles = new List<EncounterData>();
        }

        public ChapterData GetChapterData(string searchingId)
        {
            ChapterData chapter = chapters.Find(data => data.id == searchingId);

            if (chapter == null)
            {
                chapter = new ChapterData();
                chapter.id = searchingId;
                
                chapters.Add(chapter);
            }

            return chapter;
        }
        
        public EncounterData GetEventData(string searchingId)
        {
            EncounterData encounter = events.Find(data => data.id == searchingId);

            if (encounter == null)
            {
                encounter = new EncounterData();
                encounter.id = searchingId;
                
                events.Add(encounter);
            }

            return encounter;
        }
        
        public EncounterData GetBattleData(string searchingId)
        {
            EncounterData encounter = optionalBattles.Find(data => data.id == searchingId);

            if (encounter == null)
            {
                encounter = new EncounterData();
                encounter.id = searchingId;
                
                optionalBattles.Add(encounter);
            }

            return encounter;
        }
    }
}