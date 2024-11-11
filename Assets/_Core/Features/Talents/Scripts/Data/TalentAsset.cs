using System.Collections.Generic;
using Core.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Core.Features.Talents.Data
{
    [CreateAssetMenu(menuName = "Data/Talents/Asset")]
    public class TalentAsset : ScriptableObject
    {
        public Sprite sprTalentOpened;
        public Sprite sprTalentClosed;
        public List<TalentData> talentDatas;

        [Button]
        public void LoadAllData()
        {
#if UNITY_EDITOR
            talentDatas = StaticDataProvider.FindAssetsPath<TalentData>("Core/Features/Talents/Data");
#endif
        }
    }
}