using System;
using _Core.Scripts.Core.Battle.SkillScripts;
using PlayerScripts;

namespace Core.InventoryScripts
{
    public class InventorySkills_SkillDetailPresenter
    {
        public event Action<SkillConfig> OnSkillSelected;
        
        private readonly InventorySkills_SkillDetailPanelView _view;
        private SkillConfig _currentSkill;
        private SkillStorage _skillStorage;

        public  InventorySkills_SkillDetailPresenter(InventorySkills_SkillDetailPanelView view, SkillStorage skillStorage)
        {
            _view = view;
            _skillStorage = skillStorage;
        }

        public void Enable() => _view.OnSpellSlected += SelectSkill;

        public void Disable() => _view.OnSpellSlected -= SelectSkill;

        public void ShowSkillDetail(SkillConfig config)
        {
            _currentSkill = config;

            UpdateSkillView();
        }

        public void UpdateSkillView()
        {
            _view.SetManaCost(_currentSkill.manaCost);
            _view.SetCooldown(_currentSkill.cooldownMax);
            _view.SetName(_currentSkill.spellName);
            _view.SetDescription(_currentSkill.description);
            _view.SetSpellImage(_currentSkill.icon);
            _view.SetActiveButton(!_skillStorage.skillInBattle.Contains(_currentSkill));
        }

        private void SelectSkill()
        {
            OnSkillSelected?.Invoke(_currentSkill);
        }
    }
}