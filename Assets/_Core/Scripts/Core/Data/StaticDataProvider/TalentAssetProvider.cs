using System.Collections.Generic;
using Core.Data;
using Core.Features.Talents.Data;
using UnityEngine;

public class TalentAssetProvider : IStaticDataProvider
{
    public TalentAsset Asset { get; private set; }

    private Dictionary<string, TalentData> talentDatas = new();

    public TalentAssetProvider(TalentAsset asset)
    {
        this.Asset = asset;

        foreach (var data in asset.talentDatas)
        {
#if UNITY_EDITOR
            talentDatas[data.Id] = Object.Instantiate(data);
#else
            talentDatas[data.Id] = data;
#endif
            
        }
    }

    public TalentData GetTalentData(string id)
    {
        return talentDatas[id];
    }
}