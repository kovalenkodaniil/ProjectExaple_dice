using System.Collections.Generic;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace _Core.Scripts.Core.Battle.SkillScripts
{
    public class SkillCaster : MonoBehaviour
    {
        [Inject] private IObjectResolver _injectionInstantiator;
        [Inject] private Player _player;
        
        private List<SkillPresenter> _presenters = new (3);
        
        [SerializeField] private Transform _spellParent;
        [SerializeField] private SkillView _spellPrefab;

        public void Initialize()
        {
            CreateSkills();
            DisableSpells();
        }

        public void FinishBattle()
        {
            _presenters?.ForEach(presenter => presenter.Disable());
        }

        public void EnableSpells()
        {
            _presenters.ForEach(presenter => presenter.SetEnableSpell(true));
        }
        
        public void DisableSpells()
        {
            _presenters.ForEach(presenter => presenter.SetEnableSpell(false));
        }

        private void CreateSkills()
        {
            _player.skillStorage.skillInBattle.ForEach(config =>
            {
                SkillView view = Instantiate(_spellPrefab, _spellParent);

                SkillPresenter presenter = new SkillPresenter(config, view);
                _injectionInstantiator.Inject(presenter);
                
                presenter.Enable();

                _presenters.Add(presenter);
            });
        }
    }
}