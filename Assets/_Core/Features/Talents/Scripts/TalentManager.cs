using System;
using System.Collections.Generic;
using System.Linq;
using Core.Data;
using Core.Features.Talents.Data;
using Core.Saves;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace Core.Features.Talents.Scripts
{
    public class TalentManager
    {
        public event Action OnResetData;
        private readonly Lazy<Player> lazyPlayer;
        
        [Serializable]
        public class TalentProgress
        {
            public TalentData data;
            public string id;
            public int lvl;

            public bool IsOpened => data.levels.Count == lvl;

            public TalentProgress(TalentData data, string id, int lvl)
            {
                this.id = id;
                this.lvl = lvl;

                this.data = data;
            }
        }

        private Dictionary<EnumTalentEffect, List<TalentProgress>> progressDictionary = new();
        [SerializeField] private List<TalentProgress> progress;

        public TalentManager() { }

        public TalentManager(Lazy<Player> lazyPlayer)
        {
            this.lazyPlayer = lazyPlayer;

            progress = new List<TalentProgress>();
        }

        public TalentProgress LevelUpTalent(TalentData talentData)
        {
            TalentProgress progressData = progress.Find(x => x.id == talentData.Id);

            if (progressData == null)
            {
                progressData = new(StaticDataProvider.Get<TalentAssetProvider>().GetTalentData(talentData.Id), talentData.Id, 1);
                progress.Add(progressData);
                AddToProgressDictionary(progressData);
            }
            else
            {
                progressData.lvl++;
            }

            lazyPlayer.Value.model.SpendExpirience(talentData.levels[progressData.lvl - 1].exp);
            return progressData;
        }

        public int GetMaxHealth()
        {
            int maxHealth = 0;

            if (progressDictionary.TryGetValue(EnumTalentEffect.health, out var val))
            {
                maxHealth = val
                    .Where(x => x.lvl > 0)
                    .SelectMany(x => x.data.levels.Take(x.lvl))
                    .Where(x => x.effect == EnumTalentEffect.health)
                    .Sum(x => x.amount);
            }

            return maxHealth;
        }

        public int GetMaxMana()
        {
            int maxMana = 0;
            
            if (progressDictionary.TryGetValue(EnumTalentEffect.mana, out var val))
            {
                maxMana = val
                    .Where(x => x.lvl > 0)
                    .SelectMany(x => x.data.levels.Take(x.lvl))
                    .Where(x => x.effect == EnumTalentEffect.mana)
                    .Sum(x => x.amount);
            }

            return maxMana;
        }
        
        public int GetMaxFixations()
        {
            int maxFixations = 0;
            
            if (progressDictionary.TryGetValue(EnumTalentEffect.fixations, out var val))
            {
                maxFixations = val
                    .Where(x => x.lvl > 0)
                    .SelectMany(x => x.data.levels.Take(x.lvl))
                    .Where(x => x.effect == EnumTalentEffect.fixations)
                    .Sum(x => x.amount);
            }

            return maxFixations;
        }

        public bool IsMagicArrowOpened()
        {
            if (progressDictionary.TryGetValue(EnumTalentEffect.magicArrow, out var val))
            {
                return val
                    .Where(x => x.lvl > 0)
                    .SelectMany(x => x.data.levels.Take(x.lvl))
                    .Any(x => x.effect == EnumTalentEffect.magicArrow);
            }

            return false;
        }

        public bool IsMagicShieldOpened()
        {
            if (progressDictionary.TryGetValue(EnumTalentEffect.magicShield, out var val))
            {
                return val
                    .Where(x => x.lvl > 0)
                    .SelectMany(x => x.data.levels.Take(x.lvl))
                    .Any(x => x.effect == EnumTalentEffect.magicShield);
            }

            return false;
        }

        public TalentProgress GetProgress(TalentData data)
        {
            return progress.FirstOrDefault(x => x.id == data.Id);
        }

        public void Load(TalentManager loadData)
        {
            if (loadData == null)
                return;

            progress = loadData.progress ?? new(10);
            
            var provider = StaticDataProvider.Get<TalentAssetProvider>();
            foreach (var pr in progress)
                pr.data = provider.GetTalentData(pr.id);
            
            foreach (var pr in progress)
                AddToProgressDictionary(pr);
        }

        public void ResetData()
        {
            progressDictionary.Clear();
            progress.Clear();
            OnResetData?.Invoke();
        }

        private void AddToProgressDictionary(TalentProgress pr)
        {
            foreach (var lvl in pr.data.levels)
            {
                if (!progressDictionary.TryGetValue(lvl.effect, out var val))
                    progressDictionary[lvl.effect] = val = new();
                    
                val.Add(pr);
            }
        }
    }
}