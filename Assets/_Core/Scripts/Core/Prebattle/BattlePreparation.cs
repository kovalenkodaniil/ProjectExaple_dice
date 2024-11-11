using System;
using System.Collections.Generic;
using Core.Data;
using Core.InventoryScripts;
using Core.Localization;
using DG.Tweening;
using Localization;
using Managers;
using PlayerScripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Core.PreBattle
{
    public class BattlePreparation : MonoBehaviour
    {
        private const float FIGHT_ANIM_DELAY = 5f;
        private const int SPELL_IN_BATTLE_MAX = 3;
        
        [Inject] private Player _player;
        [Inject] private Managers.Localization localization;
        [Inject] private FontSettings _fontSettings;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private PopupManager _popupManager;
        [Inject] private SceneLoader _sceneLoader;

        [SerializeField] private GameObject _container;
        [SerializeField] private Transform parentDices;
        [SerializeField] private Transform parentSpells;
        [SerializeField] private Transform parentConsumables;
        [SerializeField] private DiceInBattlePreview _dicePreviewPrefab;
        [SerializeField] private InBattlePreview _spellPreviewPrefab;
        [SerializeField] private TMP_Text lbFight;
        [SerializeField] private TMP_Text lbPreparationForBattle;
        [SerializeField] private TMP_Text lbChange;
        [SerializeField] private TMP_Text lbYourSpells;
        [SerializeField] private TMP_Text lbYourConsumables;
        [SerializeField] private TMP_Text lbYourDices;
        [SerializeField] private Light _directionalLight;
        [SerializeField] private Button _closeButton;


        private List<InBattlePreview> _spellPreviews;
        private List<DiceInBattlePreview> _dicePreviews;
        private List<InBattlePreview> consumableViews = new(3);
        private Tween _labelAnimation;
        private Coroutine _waitRoutine;
        
        public event Action OnBattleReady;

        public void Open()
        {
            _container.gameObject.SetActive(true);
            
            _spellPreviews = new List<InBattlePreview>();
            _dicePreviews = new List<DiceInBattlePreview>();
            
            _closeButton.gameObject.SetActive(true);
            
            CreateStartDice();
            CreateStartSkills();
            CreateStartConsumables();

            lbFight.text = Managers.Localization.Translate(LocalizationKeys.popupBattlePrepare_fight);
            lbChange.text = Managers.Localization.Translate(LocalizationKeys.popupBattlePrepare_change);
            lbYourDices.text = Managers.Localization.Translate(LocalizationKeys.popupBattlePrepare_yourCubes);
            lbYourSpells.text = Managers.Localization.Translate(LocalizationKeys.popupBattlePrepare_yourSpells);
            lbYourConsumables.text = Managers.Localization.Translate(LocalizationKeys.popupBattlePrepare_yourItems);
            lbPreparationForBattle.text = Managers.Localization.Translate(LocalizationKeys.popupBattlePrepare_preparationForBattle);
        }

        public void Close()
        {
            _container.gameObject.SetActive(false);
            
            DisablePlayerWaiting();
            ClearContent();
        }

        public void SetActiveContainer(bool value)
        {
            _container.SetActive(value);
        }

        public void OpenCharacterScreen()
        {
            DisablePlayerWaiting();
            
            _popupManager.OpenPopup(EnumPopup.Character);

            DisableLight();
            SetActiveContainer(false);
            _popupManager._inventoryPopup.OnClosed += OnInventoryPopupClosed;
        }

        public void BackToPrevScreen()
        {
            _sceneLoader.Load(SceneEnum.Menu);
        }

        public void StartBattle()
        {
            OnBattleReady?.Invoke();
            
            Close();
        }

        private void OnInventoryPopupClosed()
        {
            _popupManager._inventoryPopup.OnClosed -= OnInventoryPopupClosed;
            
            ClearContent();

            CreateStartDice();
            CreateStartSkills();
            CreateStartConsumables();
            
            SetActiveContainer(true);
            
            _directionalLight.enabled = true;
        }
        
        private void DisableLight()
        {
            _directionalLight.enabled = false;
        }

        private void CreateStartDice()
        {
            _player.diceInBattle.ForEach(dice =>
            {
                DiceInBattlePreview dicePreview = Instantiate(_dicePreviewPrefab, parentDices);
                
                dicePreview.SetDice(dice.config);
                dicePreview.EnableTooltip(
                    dice.config.diceName,
                    _fontSettings.GetFontSetting(localization.Language, TextTag.Regular)
                    );

                _dicePreviews.Add(dicePreview);
            });
        }

        private void CreateStartSkills()
        {
            for (int i = 0; i < SPELL_IN_BATTLE_MAX; i++)
            {
                InBattlePreview spellPreview = Instantiate(_spellPreviewPrefab, parentSpells);

                if (_player.skillStorage.skillInBattle.Count > i)
                {
                    spellPreview.SetIcon(_player.skillStorage.skillInBattle[i].icon, true);
                    
                    spellPreview.EnableTooltip(
                        _player.skillStorage.skillInBattle[i].spellName,
                        _fontSettings.GetFontSetting(localization.Language, TextTag.Regular)
                    );
                }
                else
                    spellPreview.SetEmptyState();
                
                _spellPreviews.Add(spellPreview);
            }
        }
        
        private void CreateStartConsumables()
        {
            var provider = StaticDataProvider.Get<ConsumableDataProvider>();

            foreach (var view in consumableViews)
                Destroy(view.gameObject);
            
            consumableViews.Clear();

            foreach (var data in _player.inBattleConsumablesService.consumablesInBattle)
            {
                InBattlePreview prefab = Instantiate(provider.Asset.InBattlePreviewPrefab, parentConsumables);
                
                prefab.SetIcon(data.Sprite, true);
                prefab.EnableTooltip(
                    Managers.Localization.Translate(data.NameId),
                    _fontSettings.GetFontSetting(localization.Language, TextTag.Regular)
                );
                    
                consumableViews.Add(prefab);
            }
        }

        private void ClearContent()
        {
            _spellPreviews.ForEach(preview => Destroy(preview.gameObject));
            _dicePreviews.ForEach(preview => Destroy(preview.gameObject));
            
            _spellPreviews.Clear();
            _dicePreviews.Clear();
        }

        private void DisablePlayerWaiting()
        {
            if (_waitRoutine != null)
                StopCoroutine(_waitRoutine);
        }
    }
}