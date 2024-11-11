using System;
using System.Collections.Generic;

namespace Core.Saves
{
    [Serializable]
    public class SexContentSaveData
    {
        public List<SexSaveData> sexContentSaves;

        public SexContentSaveData()
        {
            sexContentSaves = new List<SexSaveData>();
        }

        public SexSaveData GetData(string id)
        {
            SexSaveData data =sexContentSaves.Find(data => data.id == id);

            if (data == null)
            {
                data = new SexSaveData();
                data.id = id;
                
                sexContentSaves.Add(data);
            }

            return data;
        }
    }

    [Serializable]
    public class SexSaveData
    {
        public string id;
        public bool isNew;
        public bool isUnlock;

        public SexSaveData()
        {
            isNew = false;
            isUnlock = false;
        }
    }
}