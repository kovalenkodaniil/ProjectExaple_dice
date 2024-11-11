using _Core.Scripts.Core.Battle.Dice;
using PlayerScripts;
using VContainer;

namespace _Core.Scripts.Core.Battle.SkillScripts
{
    public class SkillPresenter
    {
        [Inject] private TurnManager _turnManager;
        [Inject] private Player _player;
        [Inject] private DiceTower _diceTower;
        [Inject] private BattleUIPresenter _battleUIPresenter;
        
        private SkillView _view;
        private SkillConfig _config;
        private SkillData _dataSkill;
        private bool _isInteractable;
        private bool _isAvailable;

        public SkillPresenter(SkillConfig config, SkillView view)
        {
            _config = config;
            _view = view;

            _dataSkill = new SkillData(config);
        }

        public void Enable()
        {
            _isInteractable = true;
            
            InitializeSkillUI();

            _turnManager.OnStartTurn += UpdateCooldown;
            _turnManager.OnRollResult += DisableSpells;
            _view.OnCliked += UseSpell;
            _player.ManaComponent.OnValueChanged += UpdateAvailableState;
        }

        public void Disable()
        {
            _turnManager.OnStartTurn -= UpdateCooldown;
            _turnManager.OnRollResult -= DisableSpells;
            _view.OnCliked -= UseSpell;
            _player.ManaComponent.OnValueChanged -= UpdateAvailableState;
        }

        public void DisableSpells()
        {
            SetEnableSpell(false);
        }

        public void SetEnableSpell(bool isEnabling)
        {
            _isAvailable = isEnabling;
            _isInteractable = isEnabling;
            
            if (!_isAvailable)
                _view.SetDisableState(true);
            
            UpdateAvailableState(_player.ManaComponent.CurrentValue);
        }

        private void UpdateCooldown()
        {
            _dataSkill.Tick();

            _isInteractable = !_dataSkill.IsUsed;
            
            _view.IsInteractable = _isInteractable;
            
            _view.EnableCooldown(_dataSkill.IsUsed, _dataSkill.TurnForActive.ToString());
            _view.SetDisableState(true);

            UpdateAvailableState(_player.ManaComponent.CurrentValue);
        }

        private void UpdateAvailableState(int mana)
        {
            if (_dataSkill.IsUsed) return;
            if (!_isAvailable) return;
            
            _isInteractable = mana >= _config.manaCost;
            _view.IsInteractable = _isInteractable;
            _view.SetDisableState(!_isInteractable);
        }

        private void InitializeSkillUI()
        {
            _view.SkillIcon = _config.icon;
            _view.SetManaCost(_config.manaCost.ToString());
            _view.EnableCooldown(false);
            _view.SetTooltip(_config.shortDescription);
            _view.IsInteractable = _isInteractable;
        }

        private void UseSpell()
        {
            if (_player.ManaComponent.TrySpend(_config.manaCost))
            {
                _dataSkill.Use();
                _view.EnableCooldown(_dataSkill.IsUsed, _dataSkill.TurnForActive.ToString());
                
                _config.effect.Apply(_diceTower, _battleUIPresenter);
            }
        }
    }
}