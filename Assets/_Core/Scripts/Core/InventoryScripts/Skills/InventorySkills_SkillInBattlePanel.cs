using System.Collections.Generic;
using _Core.Scripts.Core.Battle.SkillScripts;
using Managers;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class InventorySkills_SkillInBattlePanel : MonoBehaviour
    {
        private SkillStorage _skillStorage;
        
        [SerializeField] private Transform _skillInBattleParent;
        [SerializeField] private InBattlePreview inBattlePreviewPrefab;

        private List<InventorySkills_SkillInBattlePresenter> _presenters;
        
        public void Initialize(SkillStorage skillStorage)
        {
            _skillStorage = skillStorage;

            CreatePreview();
        }

        public void Enable()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Initialize();
            }
        }

        public void Disable()
        {
            foreach (var presenter in _presenters)
            {
                presenter.Disable();
            }
        }

        public bool IsClickedOnPanel(Vector3 clickPosition, out InventorySkills_SkillInBattlePresenter selectedPresenter)
        {
            var worlClickPosition = GlobalCamera.Camera.ScreenToWorldPoint(clickPosition);
            
            foreach (var presenter in _presenters)
            {
                if (presenter.IsClickedOnPreview(worlClickPosition))
                {
                    selectedPresenter = presenter;
                    return true;
                }
            }
            
            selectedPresenter = null;
            return false;
        }

        public void ShowWaitingEffect(bool isShowing = true)
        {
            foreach (var presenter in _presenters)
            {
                if (isShowing)
                    presenter.Enable();
                else
                    presenter.Disable();
            }
        }

        private void CreatePreview()
        {
            _presenters = new List<InventorySkills_SkillInBattlePresenter>();
            
            foreach (var skill in _skillStorage.skillInBattle)
            {
                InBattlePreview preview = Instantiate(inBattlePreviewPrefab, _skillInBattleParent);

                InventorySkills_SkillInBattlePresenter presenter = new InventorySkills_SkillInBattlePresenter(preview, skill);
                
                _presenters.Add(presenter);
            }
        }
    }
}