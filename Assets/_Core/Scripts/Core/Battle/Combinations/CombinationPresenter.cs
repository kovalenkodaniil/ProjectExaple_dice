using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using PlayerScripts;
using VContainer;

namespace _Core.Scripts.Core.Battle.Combinations
{
    public class CombinationPresenter
    {
        [Inject] private CombinationResoverView _combinationResoverView;
        [Inject] private BattleVFXEffector _battleVFXEffector;
        [Inject] private Player _player;
        [Inject] private VFXSetting _vfxSetting;
        
        private CombinationConfig _combinationConfig;
        private List<EnumEdgeColor> _tempEdges;
        private Action _afterResolving;
        private int _effectCount;
        
        public CombinationPresenter( CombinationConfig combinationConfig)
        {
            _combinationConfig = combinationConfig;
            _tempEdges = new List<EnumEdgeColor>();
        }

        public bool TryResolveEffect(List<EnumEdgeColor> edgetypes, Action callback)
        {
            int activationsCount = 0;
            _afterResolving = callback;
            
            _tempEdges.Clear();
            _tempEdges.AddRange(edgetypes);

            if (!_combinationConfig.TryActivateCombo(_tempEdges, ref activationsCount))
            {
                _afterResolving?.Invoke();
                return false;
            }

            _effectCount = _combinationConfig.effects.Count;
            
            _combinationResoverView.PlayEffect(_combinationConfig, PlayEffect);
            return true;
        }

        private void PlayEffect()
        {
            _combinationConfig.effects.ForEach(effect =>
            {
                switch (effect.EffectType)
                {
                    case EnumEffects.Attack:
                        _battleVFXEffector.PLayPlayerAttack(
                            _vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Attack).effect,
                            _combinationResoverView.StartEffectPosition.position,
                            () =>
                            {
                                _player.AddDamage(effect.Value);
                                EndResolve();
                            });
                        break;
                    
                    case EnumEffects.Armor:
                        _battleVFXEffector.PlayArmorVFX(
                            _vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect,
                            _combinationResoverView.StartEffectPosition.position,
                            () =>
                            {
                                _player.AddArmor(effect.Value);
                                EndResolve();
                            });
                        break;
                    
                    case EnumEffects.Mana:
                        _battleVFXEffector.PlayManaVFX(
                            _vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect,
                            _combinationResoverView.StartEffectPosition.position,
                            () =>
                            {
                                _player.AddMana(effect.Value);
                                EndResolve();
                            });
                        break;
                }
            });
        }

        private void EndResolve()
        {
            _effectCount--;
            
            if (_effectCount <= 0) 
                _afterResolving?.Invoke();
        }
    }
}