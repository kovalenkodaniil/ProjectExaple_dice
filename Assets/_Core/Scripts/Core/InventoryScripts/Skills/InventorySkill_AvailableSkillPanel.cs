using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.SkillScripts;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class InventorySkill_AvailableSkillPanel : MonoBehaviour
    {
        private SkillStorage _skillStorage;

        public event Action<SkillConfig> OnSkillSelected;
        
        [SerializeField] private Transform _skillParent;
        [SerializeField] private InventorySkill_AvailableSkillPreview _skillPreview;

        private List<InventorySkill_AvailableSkillPresenter> _presenters;
        private InventorySkill_AvailableSkillPresenter _currentPresenter;
        
        public void Initialize(SkillStorage skillStorage)
        {
            _skillStorage = skillStorage;
            
            CreateInventoryCells();
        }

        public void Enable()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Enable();
                presenter.OnSkillSelected += SelectSkill;
            }

            _presenters[0].Select();
        }
        
        public void Disable()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Disable();
                presenter.OnSkillSelected -= SelectSkill;
            }
        }

        public void SetBattleMark()
        {
            _currentPresenter?.UpdateView();
        }

        private void SelectSkill(SkillConfig config, InventorySkill_AvailableSkillPresenter presenter)
        {
            if (presenter == _currentPresenter) return;
            
            _currentPresenter?.Deselect();
            _currentPresenter = presenter;
            OnSkillSelected?.Invoke(config);
        }

        private void CreateInventoryCells()
        {
            _presenters = new List<InventorySkill_AvailableSkillPresenter>();
            
            foreach (var skill in _skillStorage.availableSkills)
            {
                InventorySkill_AvailableSkillPreview preview = Instantiate(_skillPreview, _skillParent);
                InventorySkill_AvailableSkillPresenter presenter =
                    new InventorySkill_AvailableSkillPresenter(skill, preview, _skillStorage);
                
                _presenters.Add(presenter);
            }
        }
    }
}