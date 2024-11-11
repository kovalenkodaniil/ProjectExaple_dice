using System.Linq;
using Core.Data;
using Core.Features.Talents.Data;
using PlayerScripts;
using UnityEngine;

namespace Core.Features.Talents.Scripts
{
    public class TalentPresenter
    {
        private TalentData data;
        private Talent view;
        private TalentManager talentManager;
        private Player player;

        public TalentPresenter(Talent view, TalentManager talentManager, Player player)
        {
            this.talentManager = talentManager;
            this.player = player;
            this.view = view;
            data = view.Data;
            view.AddListener(HandlerOnClick);

            var progress = talentManager.GetProgress(data);
            view.SetBackgroud(GetBackgroudSprite(progress?.lvl == data.MaxLvl));
            
            view.LevelText = GetLevelText(progress?.lvl ?? 0, data.MaxLvl);

            var effectName = GetTranslate($"com_{data.Name}");
            view.DescriptionText = $"{effectName} +{data.levels[0].amount}";
        }

        private string GetTranslate(string name)
        {
            return name switch
            {
                nameof(LocalizationKeys.com_health)      => "Здоровье",
                nameof(LocalizationKeys.com_fixation)    => "Закрепы",
                nameof(LocalizationKeys.com_mana)        => "Мана",
                nameof(LocalizationKeys.com_magic_arrow)  => "Магическая стрела",
                nameof(LocalizationKeys.com_magic_shield) => "Магический щит",
                _ => ""
            };
        }

        public void Destroy()
        {
            view.RemoveListener(HandlerOnClick);
        }

        private void HandlerOnClick()
        {
            var progress = talentManager.GetProgress(data);

            if (progress != null && progress.lvl == data.MaxLvl)
                return;
            
            if (!player.model.CanSpend(data.levels[progress?.lvl ?? 0].exp))
                return;

            if (data.prerequisites.Count > 0)
            {
                bool isOpened = data.prerequisites.All(prerequisite =>
                {
                    var progressEntry = talentManager.GetProgress(prerequisite);
                    return progressEntry is { IsOpened: true };
                });

                if (!isOpened)
                    return;   
            }
            
            progress = talentManager.LevelUpTalent(view.Data);

            if (progress.IsOpened)
                view.SetBackgroud(GetBackgroudSprite(true));
                
            view.LevelText = GetLevelText(progress.lvl, data.MaxLvl);
        }

        private string GetLevelText(int currentLvl, int maxLvl)
        {
            return $"{currentLvl}/{maxLvl}";
        }

        private Sprite GetBackgroudSprite(bool isOpened)
        {
            var asset = StaticDataProvider.Get<TalentAssetProvider>().Asset;
            return isOpened ? asset.sprTalentOpened : asset.sprTalentClosed;
        }
    }
}