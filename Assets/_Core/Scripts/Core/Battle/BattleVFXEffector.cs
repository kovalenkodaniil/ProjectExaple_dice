using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Coffee.UIExtensions;
using Core.Data;
using DG.Tweening;
using Managers;
using PlayerScripts;
using UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Core.Scripts.Core.Battle
{
    public class BattleVFXEffector : MonoBehaviour
    {
        [Inject] private BattleUIPresenter _battleUIPresenter;
        [Inject] private VFXSetting _vfxSetting;
        [Inject] private Player _player;
        [Inject] private SoundManager _soundManager;

        [SerializeField] private Transform _vfxParent;
        [SerializeField] private Transform _diceCastParentPrefab;
        [SerializeField] private Canvas _battleCanvas;
        
        [Header("AttackEffect")]
        [SerializeField] private Transform _atackTrailFinish;
        [SerializeField] private UIParticle _damageEffect;
        [SerializeField] private UIParticle _damagePlayerEffect;
        
        [Header("ShieldEffect")]
        [SerializeField] private UIParticle _shieldEffect;
        [SerializeField] private UIParticle _shieldEnemyEffect;
        [SerializeField] private Animation _shieldAnimation;
        [SerializeField] private Animation _shieldEnemyAnimation;
        
        [Header("ManaEffect")]
        [SerializeField] private UIParticle _manaEffect;
        [SerializeField] private Animation _manaAnimation;
        
        [Header("HealEffect")]
        [SerializeField] private Transform _healTrailFinish;
        [SerializeField] private Transform _healEnemyTrailFinish;
        [SerializeField] private UIParticle _healEffect;
        [SerializeField] private UIParticle _healEnemyEffect;
        
        [Header("FixationEffect")]
        [SerializeField] private Transform _fixationTrailFinish;    
        [SerializeField] private Animation _fixationAnimation;
        [SerializeField] private DiceLockCounterView _fixationView;

        [Header("FireballEffect")] 
        [SerializeField] private Transform _spellFinishPosition;
        [SerializeField] private UIParticle _fireballEffect;
        
        [Header("FireballEffect")]
        [SerializeField] private UIParticle _magicArrowEffect;
        
        [Header("StunEffect")]
        [SerializeField] private UIParticle _stunEffect;
        [SerializeField] private Transform _stunTrailFinish;
        [SerializeField] private UIParticle _stunBlastEffect;
        
        [Header("LinkEffect")]
        [SerializeField] private UIParticle _linkEffect;
        [SerializeField] private Animation _linkAnimation;
        
        [Header("LoseEffect")]
        [SerializeField] private Animation _playerLoseAnimation;
        [SerializeField] private Animation _enemyLoseAnimation;

        [Header("EnemySpecialActionEffect")] 
        [SerializeField] private Image _trapEffect;
        [SerializeField] private List<Image> _diceLockIcon;

        private List<Transform> _furyList;
        private List<Transform> _defenceList;
        private List<UIParticle> _effects;

        public event Action OnLoseAnimationCompleted;
        
        # region [ AttackEffect ]
        public void PLayPlayerAttack(UIParticle prefab, Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(prefab, _vfxParent);

            Vector3 vfxPosition = _battleCanvas.transform.InverseTransformPoint(startPosition);
            vfx.transform.localPosition = new Vector3(vfxPosition.x, vfxPosition.y, 0);

            vfx.transform.DOMove(_atackTrailFinish.transform.position, 0.4f).OnComplete(() =>
            {
                vfx.Stop();
                //PlayDamageEffect();
                callback?.Invoke();
                
                Destroy(vfx.gameObject);
            });

            PlayDefaultSoundEffect();
        }
        
        public void PLayEnemyAttack(Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Attack).effect, _vfxParent);
            
            vfx.transform.position = new Vector3(startPosition.x, startPosition.y, vfx.transform.position.z);

            vfx.transform.DOMove(_healTrailFinish.transform.position, 0.3f).OnComplete(() =>
            {
                vfx.Stop();
                PlayPlayerDamageEffect();
                callback?.Invoke();
                
                Destroy(vfx.gameObject);
            });
            
            PlayDefaultSoundEffect();
        }

        public void PlayPlayerDamageEffect()
        {
            if (!_damagePlayerEffect.gameObject.activeSelf)
                _damagePlayerEffect.gameObject.SetActive(true);
            else
                _damagePlayerEffect.Play();
        }
        #endregion

        private Vector3 ShieldPosition => _battleUIPresenter.ShieldCounter.transform.position;
        private Vector3 ManaPosition => _battleUIPresenter.ManaCounter.transform.position;

        # region [ ArmorEffect ]
        public void PlayArmorVFX(UIParticle prefab, Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(prefab, _vfxParent);
            
            Vector3 vfxPosition = _battleCanvas.transform.InverseTransformPoint(startPosition);
            vfx.transform.localPosition = new Vector3(vfxPosition.x, vfxPosition.y, 0);

            vfx.transform.DOMove(ShieldPosition, 0.55f).OnComplete(() =>
            {
                vfx.Stop();
                PlayShieldEffect();
                callback?.Invoke();
                
                Destroy(vfx.gameObject);
            });
            
            PlayDefaultSoundEffect();
        }
        
        public void PlayEnemyArmorVFX(Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect, _vfxParent);

            vfx.transform.position = new Vector3(startPosition.x, startPosition.y, vfx.transform.position.z);

            vfx.transform.DOMove(ShieldPosition, 0.4f).OnComplete(() =>
            {
                vfx.Stop();
                PlayEnemyShieldEffect();
                callback?.Invoke();
                
                Destroy(vfx.gameObject);
            });
            
            PlayDefaultSoundEffect();
        }
        
        public void PlayShieldEffect()
        {
            if (!_shieldEffect.gameObject.activeSelf)
                _shieldEffect.gameObject.SetActive(true);
            else
                _shieldEffect.Play();

            _shieldAnimation.Play();
        }

        public void PlayEnemyShieldEffect()
        {
            if (!_shieldEnemyEffect.gameObject.activeSelf)
                _shieldEnemyEffect.gameObject.SetActive(true);
            else
                _shieldEnemyEffect.Play();

            _shieldEnemyAnimation.Play();
        }
        
        #endregion
        
        # region [ HealEffect ]
        public void PlayPlayerHealVFX(UIParticle prefab, Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(prefab, _vfxParent);

            Vector3 vfxPosition = _battleCanvas.transform.InverseTransformPoint(startPosition);
            vfx.transform.localPosition = new Vector3(vfxPosition.x, vfxPosition.y, 0);

            vfx.transform.DOMove(_healTrailFinish.transform.position, 0.55f).OnComplete(() =>
            {
                vfx.Stop();
                PlayHealEffect(_healEffect);
                callback?.Invoke();
                
                Destroy(vfx.gameObject);
            });
            
            PlayDefaultSoundEffect();
        }
        
        public void PlayHealEffect(UIParticle effect)
        {
            if (!effect.gameObject.activeSelf)
                effect.gameObject.SetActive(true);
            else
                effect.Play();
        }

        public void PlayEnemyHealVFX(Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Heal).effect, _vfxParent);

            vfx.transform.position = new Vector3(startPosition.x, startPosition.y, vfx.transform.position.z);

            vfx.transform.DOMove(_healEnemyTrailFinish.transform.position, 0.35f).OnComplete(() =>
            {
                vfx.Stop();
                PlayHealEffect(_healEnemyEffect);
                callback?.Invoke();
                
                Destroy(vfx.gameObject);
            });
            
            PlayDefaultSoundEffect();
        }
        
        #endregion
        
        # region [ ManaEffect ]
        public void PlayManaVFX(UIParticle prefab, Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(prefab, _vfxParent);

            Vector3 vfxPosition = _battleCanvas.transform.InverseTransformPoint(startPosition);
            vfx.transform.localPosition = new Vector3(vfxPosition.x, vfxPosition.y, 0);

            vfx.transform.DOMove(ManaPosition, 0.55f).OnComplete(() =>
            {
                vfx.Stop();
                PlayManaEffect();
                callback?.Invoke();
                
                Destroy(vfx.gameObject);
            });
            
            PlayDefaultSoundEffect();
        }
        
        public void PlayManaEffect()
        {
            if (!_manaEffect.gameObject.activeSelf)
                _manaEffect.gameObject.SetActive(true);
            else
                _manaEffect.Play();

            _manaAnimation.Play();
        }
        
        #endregion
        
        # region [ FixationEffect ]
        public void PlayFixationVFX(UIParticle prefab, Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(prefab, _vfxParent);

            Vector3 vfxPosition = _battleCanvas.transform.InverseTransformPoint(startPosition);
            vfx.transform.localPosition = new Vector3(vfxPosition.x, vfxPosition.y, 0);

            vfx.transform.DOMove(_fixationTrailFinish.transform.position, 0.55f).OnComplete(() =>
            {
                vfx.Stop();
                PlayFixationEffect();
                callback?.Invoke();
                
                Destroy(vfx.gameObject);
            });
            
            PlayDefaultSoundEffect();
        }
        
        public void PlayFixationEffect()
        {
            _fixationAnimation.Play();
        }

        public void SetFixationBlockStatus(bool isBlocking)
        {
            _fixationView.SetBlockStatus(isBlocking);
        }

        #endregion

        #region EnemyAction

        public void PlayEnemyTrapEffect(List<Vector3> dicePositions, Action callback)
        {
            _trapEffect.color = new Color(255, 255, 255, 0);
            _trapEffect.gameObject.SetActive(true);

            _trapEffect.transform.localScale = Vector3.zero;

            Sequence animationOrder = DOTween.Sequence();

            animationOrder.Append(_trapEffect.transform.DOScale(1, 0.7f));
            animationOrder.Join(_trapEffect.transform.DOScale(1, 0.7f));
            animationOrder.Append(_trapEffect.transform.DOScale(1, 0.3f));

            animationOrder.OnComplete(() =>
            {
                _trapEffect.gameObject.SetActive(false);
                
                for (int i = 0; i < dicePositions.Count; i++)
                {
                    Image lockEffect = _diceLockIcon[i];
                    lockEffect.gameObject.SetActive(true);
                    lockEffect.transform.position = _trapEffect.transform.position;
                    lockEffect.transform.DOMove(new Vector3(dicePositions[i].x, dicePositions[i].y, _diceLockIcon[i].transform.position.z), 0.6f);
                    lockEffect.transform.DOScale(0.8f, 0.4f);
                    lockEffect.transform.DOScale(0.5f, 0.2f)
                        .SetDelay(0.4f)
                        .OnComplete(() =>
                        {
                            lockEffect.gameObject.SetActive(false);
                            callback?.Invoke();
                        });
                }
            });
        }

        public void PlayEnemyStunEffect(Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect, _vfxParent);
            UIParticle vfx2 = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect, _vfxParent);
            UIParticle vfx3 = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect, _vfxParent);
            UIParticle vfx4 = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect, _vfxParent);

            vfx.transform.position = new Vector3(startPosition.x, startPosition.y, vfx.transform.position.z);
            vfx2.transform.position = new Vector3(startPosition.x, startPosition.y, vfx.transform.position.z);
            vfx3.transform.position = new Vector3(startPosition.x, startPosition.y, vfx.transform.position.z);
            vfx4.gameObject.SetActive(false);
            vfx4.transform.position = new Vector3(startPosition.x, startPosition.y, vfx.transform.position.z);

            /*vfx.transform.DOMove(_spellCaster.GetSpellPosition(0), 0.55f).OnComplete(() =>
            {
                vfx.Stop();
                PlaySpellBlockEffect(0);
                Destroy(vfx.gameObject);
            });
            
            vfx2.transform.DOMove(_spellCaster.GetSpellPosition(1), 0.55f).OnComplete(() =>
            {
                vfx2.Stop();
                PlaySpellBlockEffect(1);
                Destroy(vfx2.gameObject);
            });
            
            vfx3.transform.DOMove(_spellCaster.GetSpellPosition(2), 0.55f).OnComplete(() =>
            {
                vfx3.Stop();
                PlaySpellBlockEffect(2);
                Destroy(vfx3.gameObject);
            });*/

            vfx4.transform.DOMove(_fixationTrailFinish.position, 0.55f)
                .SetDelay(0.55f)
                .OnStart(() => vfx4.gameObject.SetActive(true))
                .OnComplete(() =>
                {
                    vfx4.Stop();
                    PlayFixationBlockEffect();
                    Destroy(vfx4.gameObject);
                    callback?.Invoke();
                });
        }

        private void PlaySpellBlockEffect(int index)
        {
            //_spellCaster.PlayLockAnimation(index);
        }

        private void PlayFixationBlockEffect()
        {
            PlayFixationEffect();
            SetFixationBlockStatus(true);
        }

        #endregion
        
        # region [ LoseEffect ]
        public void PlayPlayerLoseEffect(Action callback)
        {
            _playerLoseAnimation.gameObject.SetActive(true);
            _playerLoseAnimation.Play();

            StartCoroutine(WaitAnimCompleted(callback));
        }
        
        public void PlayEnemyLoseEffect(Action callback)
        {
            _enemyLoseAnimation.gameObject.SetActive(true);
            _enemyLoseAnimation.Play();

            StartCoroutine(WaitAnimCompleted(callback));
        }

        private IEnumerator WaitAnimCompleted(Action callback)
        {
            yield return new WaitForSeconds(2.4f);
            
            OnLoseAnimationCompleted?.Invoke();
            callback?.Invoke();
            
            _enemyLoseAnimation.gameObject.SetActive(false);
            _playerLoseAnimation.gameObject.SetActive(false);
        }

        #endregion
        
        # region [ SpellEffect ]
        public void PLayBarrierAnimation(Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect, _vfxParent);
            UIParticle vfx2 = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect, _vfxParent);
            UIParticle vfx3 = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Armor).effect, _vfxParent);
            
            vfx.transform.position = startPosition;
            vfx2.transform.position = startPosition;
            vfx2.gameObject.SetActive(false);
            vfx3.transform.position = startPosition;
            vfx3.gameObject.SetActive(false);

            vfx.transform.DOMove(ShieldPosition, 0.4f).OnComplete(() =>
            {
                vfx.Stop();
                Destroy(vfx.gameObject);
                _shieldAnimation.Play();
                
                callback?.Invoke();
            });
            vfx2.transform.DOMove(ShieldPosition, 0.4f).SetDelay(0.55f).OnStart(() =>
            {
                vfx2.gameObject.SetActive(true);
            }).OnComplete(() =>
            {
                vfx2.Stop();
                Destroy(vfx2.gameObject);
                _shieldAnimation.Play();
                
                callback?.Invoke();
            });
            vfx3.transform.DOMove(ShieldPosition, 0.4f).SetDelay(1.1f).OnStart(() =>
                {
                    vfx3.gameObject.SetActive(true);
                })
                .OnComplete(() =>
                {
                    vfx3.Stop();
                    PlayShieldEffect();
                    callback?.Invoke();
                
                    Destroy(vfx3.gameObject);
                });
        }

        public void PLayMasterryAnimation(Vector3 startPosition, Action callback)
        {
            UIParticle vfx = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Skill).effect, _vfxParent);
            UIParticle vfx2 = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Skill).effect, _vfxParent);
            UIParticle vfx3 = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Skill).effect, _vfxParent);
            
            vfx.transform.position = startPosition;
            vfx2.transform.position = startPosition;
            vfx2.gameObject.SetActive(false);
            vfx3.transform.position = startPosition;
            vfx3.gameObject.SetActive(false);

            vfx.transform.DOMove(_fixationTrailFinish.transform.position, 0.4f).OnComplete(() =>
            {
                vfx.Stop();
                Destroy(vfx.gameObject);

                PlayFixationEffect();
                
                callback?.Invoke();
            });
            vfx2.transform.DOMove(_fixationTrailFinish.transform.position, 0.4f).SetDelay(0.55f).OnStart(() =>
            {
                vfx2.gameObject.SetActive(true);
            }).OnComplete(() =>
            {
                vfx2.Stop();
                Destroy(vfx2.gameObject);
                
                PlayFixationEffect();
                
                callback?.Invoke();
            });
            vfx3.transform.DOMove(_fixationTrailFinish.transform.position, 0.4f).SetDelay(1.1f).OnStart(() =>
                {
                    vfx3.gameObject.SetActive(true);
                })
                .OnComplete(() =>
                {
                    vfx3.Stop();
                    
                    PlayFixationEffect();
                    callback?.Invoke();
                
                    Destroy(vfx3.gameObject);
                });
        }

        public void PLayFireballAnimation(Vector3 startPosition, Action callback)
        {
            UIParticle trail = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.FireballTrail).effect, _vfxParent);
            
            trail.transform.position = startPosition;
            
            trail.transform.DOMove(_spellFinishPosition.position, 0.4f).OnComplete(() =>
            {
                trail.Stop();
                callback?.Invoke();
                
                if (!_fireballEffect.gameObject.activeSelf)
                    _fireballEffect.gameObject.SetActive(true);
                else
                    _fireballEffect.Play();

                Destroy(trail.gameObject);
            });
        }
        
        public void PLayMagicArrowAnimation(Vector3 startPosition, Action callback)
        {
            UIParticle trail = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.MagicArrowTrail).effect, _vfxParent);
            
            trail.transform.position = startPosition;
            
            trail.transform.DOMove(_spellFinishPosition.position, 0.4f).OnComplete(() =>
            {
                trail.Stop();
                callback?.Invoke();
                
                if (!_magicArrowEffect.gameObject.activeSelf)
                    _magicArrowEffect.gameObject.SetActive(true);
                else
                    _magicArrowEffect.Play();

                Destroy(trail.gameObject);
            });
        }
        
        public void PLayStunAnimation(Vector3 startPosition, Vector3 targetPosition, Action callback)
        {
            UIParticle trail = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Mana).effect, _vfxParent);
            
            trail.transform.position = startPosition;
            
            trail.transform.DOMove(targetPosition, 0.4f).OnComplete(() =>
            {
                trail.Stop();
                
                if (!_stunBlastEffect.gameObject.activeSelf)
                    _stunBlastEffect.gameObject.SetActive(true);
                else
                    _stunBlastEffect.Play();

                Destroy(trail.gameObject);
            });

            _stunBlastEffect.transform.DOScale(0, 0f).SetRelative().SetDelay(0.6f).OnComplete(() =>
            {
                if (!_stunEffect.gameObject.activeSelf)
                    _stunEffect.gameObject.SetActive(true);
                
                _stunEffect.Play();
            });
            
            _stunBlastEffect.transform.DOScale(0, 0f).SetRelative().SetDelay(1f).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }
        
        public void PLayLinkAnimation()
        {
            _linkAnimation.gameObject.SetActive(true);
            _linkAnimation.Play();

            StartCoroutine(WaitDelay(0.8f, () =>
            {
                if (!_linkEffect.gameObject.activeSelf)
                    _linkEffect.gameObject.SetActive(true);
                else
                    _linkEffect.Play();
            }));
        }

        public void PlayFuryCastAnimation(Transform spell)
        {
            UIParticle cast = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.FuryCast).effect, spell);
            
            cast.transform.localPosition = new Vector3(cast.transform.localPosition.x, cast.transform.localPosition.y, 0);
                    
            cast.Play();
        }
        
        public void PlayBastionCastAnimation(Transform spell)
        {
            UIParticle cast = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.DefenceCast).effect, spell);
            
            cast.transform.localPosition = new Vector3(cast.transform.localPosition.x, cast.transform.localPosition.y, 0);
                    
            cast.Play();
        }

        public bool PlayFuryAnimation(List<Dice.Dice> diceList, DiceTower diceTower)
        {
            if (_player.DamageComponent.DamageBuf == 0) return false;

            if (_furyList == null)
                _furyList = new List<Transform>();
            else
                _furyList.Clear();
            
            diceList.ForEach(dice =>
            {
                if (dice._data.CurrentEdge.edgePattern._effects.Exists(effect => effect.EffectType == EnumEffects.Attack))
                {
                    Transform parent = Instantiate(_diceCastParentPrefab, transform);
                    UIParticle cast = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Fury).effect, parent);

                    Vector3 vfxPosition = _battleCanvas.transform.InverseTransformPoint(dice.transform.position);

                    parent.localPosition = new Vector3(vfxPosition.x, vfxPosition.y, -47.2f);
                    cast.transform.localPosition = Vector3.zero;
                    
                    cast.Play();
                    
                    _furyList.Add(parent);
                }
            });
            
            return true;
        }
        
        public bool PlayDefenceAnimation(List<Dice.Dice> diceList, DiceTower diceTower)
        {
            if (_player.ArmorComponent.ArmorBuf == 0) return false;
            
            if (_defenceList == null)
                _defenceList = new List<Transform>();
            else
                _defenceList.Clear();
            
            diceList.ForEach(dice =>
            {
                if (dice._data.CurrentEdge.edgePattern._effects.Exists(effect => effect.EffectType == EnumEffects.Armor))
                {
                    Transform parent = Instantiate(_diceCastParentPrefab, transform);
                    UIParticle cast = Instantiate(_vfxSetting.battleEffects.Find(effect => effect.effectType == EnumVFXEffect.Defence).effect, parent);

                    Vector3 vfxPosition = _battleCanvas.transform.InverseTransformPoint(dice.transform.position);

                    parent.localPosition = new Vector3(vfxPosition.x, vfxPosition.y, -47.2f);
                    cast.transform.localPosition = Vector3.zero;

                    cast.Play();
                    
                    _defenceList.Add(parent);
                }
            });
            
            return true;
        }

        public void DisableFuryEffect()
        {
            if (_furyList == null || _furyList.Count == 0) return;

            _furyList?.ForEach(parent =>
            {
                parent.DOScale(0, 1.5f)
                    .SetLink(parent.gameObject)
                    .SetDelay(0.4f)
                    .OnComplete(() => Destroy(parent.gameObject));
            });
        }
        
        public void DisableBastionEffect()
        {
            if (_defenceList == null || _defenceList.Count == 0) return;

            _defenceList?.ForEach(parent =>
            {
                parent.DOScale(0, 1.5f)
                    .SetLink(parent.gameObject)
                    .SetDelay(0.4f)
                    .OnComplete(() => Destroy(parent.gameObject));
            });
        }

        public void OffEnemyStunEffect()
        {
            return;
            
            if (_stunEffect.gameObject.activeSelf)
            {
                _stunEffect.Stop();
                _stunEffect.gameObject.SetActive(false);
            }
            
            _defenceList?.Clear();
            _furyList?.Clear();
        }

        private IEnumerator WaitDelay(float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            
            callback?.Invoke();
        }

        #endregion

        private void PlayDefaultSoundEffect()
        {
            _soundManager.PlayEffect(_soundManager.SoundList.HitSound);
        }
    }
}