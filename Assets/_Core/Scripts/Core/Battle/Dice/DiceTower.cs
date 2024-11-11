using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Combinations;
using _Core.Scripts.Core.Battle.Enemies;
using Core.Data;
using Core.Effects;
using Managers;
using PlayerScripts;
using Popups;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Effect = Core.Data.Effect;
using Random = UnityEngine.Random;

namespace _Core.Scripts.Core.Battle.Dice
{
    public class DiceTower : MonoBehaviour
    {
        [Inject] private Player _player;
        [Inject] private TurnManager _turnManager;
        [Inject] private IObjectResolver _injectionInstantiator;
        [Inject] private BattleUIPresenter _battleUIPresenter;
        [Inject] private BattleVFXEffector _battleVFXEffector;
        [Inject] private VFXSetting _vfxSetting;
        [Inject] private CombinationResolver _combinationResolver;
        [Inject] private EnemyBattleManager _enemyBattleManager;
        [Inject] private EffectManager effectManager;
        
        [SerializeField] private Dice _dicePrefab;
        [SerializeField] private Transform _dicePanel;
        [SerializeField] private Vector2[] _diceStartPositions;
        [SerializeField] private float _diceStartPositionZ;
        [SerializeField] private List<AnimationClip> _tutorialClips;
        [SerializeField] private PopupChangeColor _popupChangeColor;

        public DiceChecker diceChecker;
        private int _holdCount;
        public List<Dice> _diceList;
        private int _resolveCount;
        private bool _isFirstRoll;
        private bool _isTutoriaMode;
        private bool _isWaitEffect;
        private Coroutine _resolveCoroutine;
        public int attackBuff;
        public int armorBuff;
        
        public event Action OnRollResolved;

        public void Initialize()
        {
            _holdCount = 1;
            _isFirstRoll = true;
            _isTutoriaMode = false;

            diceChecker = new DiceChecker();
            
            CreateDice();
            _battleUIPresenter.SetDices(_diceList);

            RollDice();
        }

        public void OnDisable()
        {
            Unsub();
        }

        public void Unsub()
        {
            _combinationResolver.OnAllCombinationsResolved -= FinishDiceResolve;
            
            _diceList?.ForEach(dice =>
            {
                dice.OnLand -= CheckAllDiceDropped;
                dice.OnReturn -= CheckAllDiceReturned;
            });
        }

        public void FinishBattle()
        {
            if (_resolveCoroutine != null)
                StopCoroutine(_resolveCoroutine);

            Unsub();
        }

        public void PrepareNewTurn()
        {
            _diceList.ForEach(dice =>
            {
                dice.diceHolder.CanBeHold = true;
                dice.diceHolder.UnHold();
                dice.Reset();
            });
        }

        public void RollDice()
        {
            if (_isTutoriaMode) return;

            if (_resolveCount >= 6)
            {
                CheckAllDiceReturned();
                return;
            }

            _resolveCount = 0;
            
            int rollCount = 0;
            
            for (int i = 0; i < _diceList.Count; i++)
            {
                if (_diceList[i].diceHolder.IsBlocking) continue;
                if (_diceList[i].diceHolder.IsHeld) continue;

                rollCount++;
                
                _diceList[i].Roll(rollCount, i+1);
            }
            
            
        }
        

        public void RollSingleDice(Dice dice)
        {
            dice.diceHolder.UnHold(() =>
            {
                dice.Roll(1, _diceList.IndexOf(dice)+1, () => dice.PlayReturnAnimation());
            });
        }

        public void ShowColorPopup(Dice dice)
        {
            _popupChangeColor.Open(dice);
        }

        public void TryUnblockDice()
        {
            _diceList.ForEach(dice =>
            {
                if (dice.isBlocking)
                {
                    dice.BlockDice(false);
                    dice.SetBlockingView(false);
                }
            });
        }

        private void CreateDice()
        {
            _diceStartPositions = _battleUIPresenter.GetDiceStartPositions();
            
            if (_diceList == null)
                _diceList = new List<Dice>();
            else
            {
                _diceList.ForEach(dice => Destroy(dice.gameObject));
                _diceList.Clear();
            }

            for (int i = 0; i < _player.diceInBattle.Count; i++)
            {
                var dice =_injectionInstantiator.Instantiate(_dicePrefab, _dicePanel);

                BattleDiceData battleDiceData = new BattleDiceData(_player.diceInBattle[i].config);
                
                dice.Initialize(battleDiceData, new Vector3(_diceStartPositions[i].x, _diceStartPositions[i].y, _diceStartPositionZ));
                _diceList.Add(dice);
                
                dice.OnLand += CheckAllDiceDropped;
                dice.OnReturn += CheckAllDiceReturned;
            }
        }

        private void CheckAllDiceDropped()
        {
            _resolveCount++;

            if (_resolveCount >= 6)
            {
                _resolveCount = 0;

                PlayDiceReturn();
            }
        }
        
        private void CheckAllDiceReturned()
        {
            _resolveCount++;
            
            if (_resolveCount >= 6)
            {
                _resolveCount = 0;

                if (_isFirstRoll)
                {
                    _isFirstRoll = false;
                    StartCoroutine(WaitFirstRoll());
                    return;
                }
            }
        }

        private void PlayDiceReturn()
        {
            float returnDelay = 0.2f;
            
            for (int i = 0; i < _diceList.Count; i++)
            {
                if (_diceList[i].diceHolder.IsBlocking || _diceList[i].diceHolder.IsHeld)
                {
                    _resolveCount++;
                    continue;
                }

                _diceList[i].PlayReturnAnimation(returnDelay);

                returnDelay += 0.3f;
            }
            
            if (_resolveCount >= 6) CheckAllDiceReturned();
        }

        public void ResolveDice()
        {
            List<EmitPrioritySetting> currentDiceEffect = new List<EmitPrioritySetting>();
            
            foreach (var dice in _diceList)
            {
                if (dice.isBlocking) continue;
                
                dice._data.CurrentEdge.edgePattern._effects.ForEach(effect =>
                {
                    currentDiceEffect.Add(new EmitPrioritySetting(effect, dice));
                });
            }
            
            _resolveCoroutine = StartCoroutine(ResolveCoroutine(currentDiceEffect));
        }

        private IEnumerator ResolveCoroutine(List<EmitPrioritySetting> sortedDice)
        {
            _isWaitEffect = false;

            if (_battleVFXEffector.PlayDefenceAnimation(_diceList, this))
                yield return new WaitForSeconds(0.2f);

            if (_battleVFXEffector.PlayFuryAnimation(_diceList, this))
                yield return new WaitForSeconds(0.2f);

            foreach (var effectType in _vfxSetting.emitOrder)
            {
                List<EmitPrioritySetting> tempList = sortedDice.FindAll(emitSetting => emitSetting.effect.EffectType == effectType);
                
                if (tempList.Count == 0) continue;

                foreach (var emit in tempList)
                {
                    yield return new WaitForSeconds(0.55f);
                    
                    emit.dice.PlayCastAnimation();
                    
                    switch (emit.effect.EffectType)
                            {
                                case EnumEffects.Attack:
                                    _battleVFXEffector.PLayPlayerAttack(
                                        _vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Attack).effect,
                                        emit.dice.transform.position,
                                        () => _player.AddDamage(emit.effect.Value + attackBuff)
                                        );
                                    break;
                    
                                case EnumEffects.Armor:
                                    _battleVFXEffector.PlayArmorVFX(
                                        _vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect,
                                        emit.dice.transform.position,
                                        () => _player.AddArmor(emit.effect.Value + armorBuff)
                                        );
                                    break;

                                case EnumEffects.Mana:
                                    _battleVFXEffector.PlayManaVFX(
                                        _vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Mana).effect,
                                        emit.dice.transform.position,
                                        () => _player.AddMana(emit.effect.Value)
                                    );
                                    break;
                        
                                case EnumEffects.NoLethalDamage:
                                    _player.TakeNoLethalDamage(emit.effect.Value);
                                    break;
                            
                                case EnumEffects.Heal:
                                    _battleVFXEffector.PlayPlayerHealVFX(
                                        _vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Heal).effect,
                                        emit.dice.transform.position,
                                        () => _player.HealthComponent.Heal(emit.effect.Value)
                                    );
                                    break;

                                case EnumEffects.Stun:
                                    _isWaitEffect = true;
                                    WaitStun(emit);
                                    break;
                                
                                case EnumEffects.AttackAll:
                                    _battleVFXEffector.PLayFireballAnimation(
                                        emit.dice.transform.position,
                                        () => _enemyBattleManager.Enemies.ForEach(enemy => enemy.model.TakeDamage(emit.effect.Value))
                                    );
                                    break;
                    
                                default:
                                    break;
                            }

                    yield return new WaitUntil(() => !_isWaitEffect);
                }

                switch (effectType)
                {
                    case EnumEffects.Armor:
                        _battleVFXEffector.DisableBastionEffect();
                        break;
                    
                    case EnumEffects.Attack:
                        _battleVFXEffector.DisableFuryEffect();
                        break;
                }
                
                yield return new WaitForSeconds(0.6f);
            }

            if (!_isWaitEffect)
                StartCombinationResolving();
        }

        private void StartCombinationResolving()
        {
            _combinationResolver.OnAllCombinationsResolved += FinishDiceResolve;
            
            _combinationResolver.ResolveCombination(_diceList);
        }
        
        private void FinishDiceResolve()
        {
            _combinationResolver.OnAllCombinationsResolved -= FinishDiceResolve;

            OnRollResolved?.Invoke();
        }
        
        private IEnumerator WaitFirstRoll()
        {
            yield return new WaitForSeconds(1f);
            
            _diceList.ForEach(dice =>
            {
                dice.diceHolder.CanBeHold = true;
                dice.IsDropped = true;
                dice.diceHolder.UnHold();
            });
        }
        
        public void WaitStun(EmitPrioritySetting emitPrioritySetting)
        {
            _battleUIPresenter.SetInteractableButtons(false);
            _enemyBattleManager.Enemies.ForEach(enemy => enemy.view.EnableIntentionsBlinking(true));
            
            CoroutineManager.StartCoroutine(WaitSelect(emitPrioritySetting));

            IEnumerator WaitSelect(EmitPrioritySetting emit)
            {
                while (true)
                {
                    if (Input.GetMouseButton(0))
                    {
                        var worldMousePosition = GlobalCamera.Camera.ScreenToWorldPoint(Input.mousePosition);

                        if (_enemyBattleManager.IsMouseOverIntention(worldMousePosition, out var combatEntity, out int intentionIndex, out Vector3 targetPosition))
                        {
                            _battleVFXEffector.PLayStunAnimation(emit.dice.transform.position, targetPosition,null);
                            combatEntity.AddEffect(effectManager.GetEffect(EnumEffects.StunIntention), intentionIndex);
                            
                            _battleUIPresenter.SetInteractableButtons(true);
                            _enemyBattleManager.Enemies.ForEach(enemy =>
                            {
                                enemy.view.EnableIntentionsBlinking(false);
                                
                                if (enemy.model == combatEntity)
                                    enemy.view.HideIntention(intentionIndex);

                                _isWaitEffect = false;
                            });
                            yield break;   
                        }
                    }

                    yield return null;
                }
            }
        }
    }
    
    public class EmitPrioritySetting
    {
        public Effect effect;
        public Dice dice;

        public EmitPrioritySetting(Effect effect, Dice dice)
        {
            this.effect = effect;
            this.dice = dice;
        }
    }
}