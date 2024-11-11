using System.Collections.Generic;
using Core.Items.ItemEffects;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace Core.Items
{
    public class ItemManager
    {
        [Inject] private Player _player;
        [Inject] private IObjectResolver _objectResolver;

        private List<ItemEffect> _effects;
        
        public void Initialize()
        {
            InitializeEffectItems();
        }

        public void FinishBattle()
        {
            _effects?.ForEach(effect => effect.Reset());
        }

        private void InitializeEffectItems()
        {
            _effects = new List<ItemEffect>();
            
            _player.itemStorage.itemInBattle.ForEach(item =>
            {
                switch (item.type)
                {
                    case ItemType.TrumpСard:
                        TrumpCardEffect trumpCard = new TrumpCardEffect();
                        _objectResolver.Inject(trumpCard);
                        trumpCard.Initialize(item);
                        
                        _effects.Add(trumpCard);
                        break;
                    
                    case ItemType.TheBronzeHorseshoe:
                        TheBronzeHorseshoeEffect TheBronzeHorseshoe = new TheBronzeHorseshoeEffect();
                        _objectResolver.Inject(TheBronzeHorseshoe);
                        TheBronzeHorseshoe.Initialize(item);
                        
                        _effects.Add(TheBronzeHorseshoe);
                        break;
                }
            });
        }
    }
}