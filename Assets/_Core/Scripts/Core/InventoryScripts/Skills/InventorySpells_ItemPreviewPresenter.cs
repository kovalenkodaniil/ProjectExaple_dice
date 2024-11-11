using System;
using _Core.Scripts.Core.Battle.SkillScripts;
using Core.Data;
using Core.Localization;
using PlayerScripts;
using VContainer;

namespace Core.InventoryScripts
{
    public class InventorySpells_ItemPreviewPresenter
    {
        [Inject] private Player _player;
        [Inject] private Managers.Localization localization;
        [Inject] private FontSettings _fontSettings;
        
        private InventorySkills_SkillDetailPanelView _inventorySkillsSkillDetailPanelView;
        private InventoryPreview _preview;
        private SkillConfig _spellConfig;

        public event Action<InventorySpells_ItemPreviewPresenter> OnPreviewClicked;
        public event Action<SkillConfig> OnSpellSelectedForBattle;

        public InventorySpells_ItemPreviewPresenter(InventorySkills_SkillDetailPanelView inventorySkillsSkillDetailPanelView, InventoryPreview preview,
            SkillConfig spellConfig)
        {
            this._inventorySkillsSkillDetailPanelView = inventorySkillsSkillDetailPanelView;
            _preview = preview;
            _spellConfig = spellConfig;
        }

        public void Enable()
        {
            UpdatePreview();
            UpdateInventoryPreview();
            SetInventoryPreviewLocalization();

            _preview.OnSpellPreviewClicked += Select;
            _preview.OnPreviewDragStarted += TryDragSpell;
        }

        public void Disable()
        {
            _preview.OnSpellPreviewClicked -= Select;
            _preview.OnPreviewDragStarted -= TryDragSpell;
            _inventorySkillsSkillDetailPanelView.OnSpellSlected -= AddSpellSpellInBattle;
        }

        public void SelectFirst()
        {
            UpdateInventoryPreview();
            SetSelectingView();
            
        }

        public void Select()
        {
            _inventorySkillsSkillDetailPanelView.PlayHideContent(UpdateInventoryPreview);
            SetSelectingView();
            //_inventoryView.Icon
        }

        public void TryDragSpell()
        {
            Select();
            
            _inventorySkillsSkillDetailPanelView.AddSpellInBattle();
        }

        private void SetSelectingView()
        {
            _preview.SetIcon(_spellConfig.icon);
            _preview.SetSelectingVariant();
            
            OnPreviewClicked?.Invoke(this);
            
            _inventorySkillsSkillDetailPanelView.OnSpellSlected -= AddSpellSpellInBattle;
            _inventorySkillsSkillDetailPanelView.OnSpellSlected += AddSpellSpellInBattle;
        }

        public void OffPreview()
        {
            UpdatePreview();
            _preview.HideSelectingEffect();

            _inventorySkillsSkillDetailPanelView.OnSpellSlected -= AddSpellSpellInBattle;
        }

        private void UpdateInventoryPreview()
        {
            _inventorySkillsSkillDetailPanelView.SetSpellImage(_spellConfig.icon);
            _inventorySkillsSkillDetailPanelView.SetName(localization.GetTranslate(_spellConfig.spellName));
            _inventorySkillsSkillDetailPanelView.SetDescription(localization.GetTranslate(_spellConfig.description));
            _inventorySkillsSkillDetailPanelView.SetManaCost(_spellConfig.manaCost);
            _inventorySkillsSkillDetailPanelView.SetCooldown(_spellConfig.cooldownMax);
            
            if (_player.skillStorage.availableSkills.Contains(_spellConfig))
                _inventorySkillsSkillDetailPanelView.SetActiveButton(!_player.skillStorage.skillInBattle.Contains(_spellConfig));
            else
                _inventorySkillsSkillDetailPanelView.SetActiveButton(false);
            
            _inventorySkillsSkillDetailPanelView.PlayAppearanceContent();
        }

        public void UpdatePreview()
        {
            if (!_player.skillStorage.availableSkills.Contains(_spellConfig))
            {
                _preview.SetIcon(_spellConfig.icon);
                _preview.SetBlockingVariant();
                _preview.SetDragable(false);
                
                return;
            }

            if (_player.skillStorage.skillInBattle.Contains(_spellConfig))
            {
                _preview.SetIcon(_spellConfig.icon);
                _preview.SetInBattleVariant();
                _preview.Icon.color = _spellConfig.InBattlePreviewColor;
                _preview.SetDragable(false);
            }
            else
            {
                _preview.SetIcon(_spellConfig.icon);
                _preview.SetDefaultVariant(false);
                _preview.SetDragable(true);
            }
        }

        private void SetInventoryPreviewLocalization()
        {
            _preview.SetLocalization(localization, _fontSettings);
        }

        private void AddSpellSpellInBattle()
        {
            OnSpellSelectedForBattle?.Invoke(_spellConfig);
        }
    }
}