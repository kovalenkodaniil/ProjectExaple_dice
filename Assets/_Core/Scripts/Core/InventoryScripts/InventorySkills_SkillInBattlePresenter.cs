using System;
using _Core.Scripts.Core.Battle.SkillScripts;
using Core.Data;
using PlayerScripts;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class InventorySkills_SkillInBattlePresenter
    {
        private InBattlePreview _preview;
        private SkillConfig _skillConfig;
        private SkillConfig _newSkillConfig;
        private Player _player;

        public SkillConfig CurrentSkill => _skillConfig;

        public event Action OnSpellAdded;

        public InventorySkills_SkillInBattlePresenter(InBattlePreview preview, SkillConfig config)
        {
            _skillConfig = config;
            _preview = preview;
        }

        public void Initialize()
        {
            UpdatePreview();
        }

        public void Enable()
        {
            _preview.SetWaitEffect(true);
        }

        public void Disable()
        {
            _preview.SetWaitEffect(false);
        }

        public bool IsClickedOnPreview(Vector3 clickPosition)
        {
            var localClickPosition = _preview.RectTransform.InverseTransformPoint(clickPosition);
            
            return _preview.RectTransform.rect.Contains(localClickPosition);
        }

        public void ReplaceSkill(SkillConfig config)
        {
            if (_skillConfig != null)
            {
                _preview.PLayHideAnimation(() =>
                {
                    UpdatePreview(false);
                });
            }
            else
            {
                UpdatePreview(false);
            }
            
            _skillConfig = config;
        }

        private void UpdatePreview(bool isFirstUpdate = true)
        {
            if (_skillConfig == null)
            {
                _preview.SetEmptyState();
                return;
            }

            _preview.SetIcon(_skillConfig.icon, isFirstUpdate);
        }
    }
}