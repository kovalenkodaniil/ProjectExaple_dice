using System;
using _Core.Scripts.Core.Battle.SkillScripts;

namespace Core.InventoryScripts
{
    public class InventorySkill_AvailableSkillPresenter
    {
        public event Action<SkillConfig, InventorySkill_AvailableSkillPresenter> OnSkillSelected;

        private SkillStorage _skillStorage;
        private InventorySkill_AvailableSkillPreview _view;
        private SkillConfig _config;

        public InventorySkill_AvailableSkillPresenter(SkillConfig config, InventorySkill_AvailableSkillPreview view, SkillStorage skillStorage)
        {
            _view = view;
            _config = config;
            _skillStorage = skillStorage;
        }

        public void Enable()
        {
            UpdateView();
            _view.OnSkillPreviewClicked += SelectSkill;
        }

        public void Disable()
        {
            UpdateView();
            _view.OnSkillPreviewClicked -= SelectSkill;
        }

        public void Deselect()
        {
            _view.HideSelectingEffect();
            UpdateViewVariant();
        }

        public void Select() => SelectSkill();

        public void UpdateView()
        {
            _view.Icon.sprite = _config.icon;

            UpdateViewVariant();
        }

        private void UpdateViewVariant()
        {
            if (_skillStorage.skillInBattle.Contains(_config))
                _view.SetInBattleVariant();
            else
                _view.SetDefaultVariant();
        }

        private void SelectSkill()
        {
            SetSelectingView();
            OnSkillSelected?.Invoke(_config, this);
        }

        private void SetSelectingView()
        {
            _view.SetSelectingVariant();
        }
    }
}