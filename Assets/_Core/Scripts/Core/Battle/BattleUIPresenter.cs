using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Consumables;
using _Core.Scripts.Core.Battle.Dice;
using _Core.Scripts.Core.Battle.SkillScripts;
using Core.Data;
using Core.InventoryScripts.Items;
using Core.Localization;
using Localization;
using Managers;
using PlayerScripts;
using UI;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace _Core.Scripts.Core.Battle
{
    public class BattleUIPresenter : MonoBehaviour
    {
        [Inject] private Player _player;
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private PopupManager _popupManager;
        [Inject] private VFXSetting _vfxSetting;
        [Inject] private Managers.Localization localization;
        [Inject] private FontSettings _fontSettings;
        [Inject] private SoundManager _soundManager;
        [Inject] private TurnManager turnManager;
        
        [SerializeField] private Canvas _battleCanvas;
        [SerializeField] private ItemsBattleView _battleView;
        [SerializeField] private SkillInfoHelper _infoHelper;
        
        [Header("PlayerUI")]
        [SerializeField] private GameObject _UIContiner;
        [SerializeField] private StatBar _playerHp;
        [SerializeField] private BattleArmourCounterPresenter armourCounter;
        [SerializeField] private BattleDamageCounterPresenter damageCounter;
        [SerializeField] private StatBar _playerMana;
        [SerializeField] private List<DiceCell> _diceCells;
        [SerializeField] private TooltipWindow _diceTooltip;
        [SerializeField] private StatCounter manaCounter;

        [Header("Buttons")]
        [SerializeField] private Button _rollButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _endTurnButton;

        [SerializeField] private GameObject anticlick;
        
        [SerializeField] private ConsumablesBattleView consumables;
        
        private List<DiceHolder> _diceHolders;

        public event Action OnRollButtonClicked;
        public event Action OnEndTurnButtonClicked;

        public BattleArmourCounterPresenter ShieldCounter => armourCounter;
        public StatCounter ManaCounter => manaCounter;

        private void Start()
        {
            SetActiveAnticlick(false);
            SetSkillInfo(false);
            _battleView.Initialize(_player.itemStorage);
        }

        public void OnDisable()
        {
            if (_diceHolders == null || _diceHolders.Count <= 0) return;
            
            for (int i = 0; i < _diceHolders.Count; i++)
            {
                _diceHolders[i].OnHoldFinished -= _diceCells[i].EnableLockEffect;
                _diceHolders[i].OnUnHoldFinished -= _diceCells[i].DisableLockEffect;
            }
            
            armourCounter.Disabe();
            damageCounter.Disabe();
        }

        public Vector2[] GetDiceStartPositions()
        {
            Vector2[] startPositions = new Vector2[6];
            
            for (int i=0; i < _diceCells.Count; i++)
            {
                startPositions[i] = new Vector2(_diceCells[i].transform.position.x, _diceCells[i].transform.position.y);
            }

            return startPositions;
        }

        public void InitializeUI()
        {
            _UIContiner.gameObject.SetActive(true);
            _pauseButton.gameObject.SetActive(true);
            
            _objectResolver.Inject(consumables);
            consumables.Construct(_player, turnManager, _player.inBattleConsumablesService.consumablesInBattle);
        }

        public void SetSkillInfo(bool isShowing, string info = "")
        {
            _infoHelper.gameObject.SetActive(isShowing);
            _infoHelper.InfoText = info;
        }

        public void SetActiveAnticlick(bool value)
        {
            anticlick.SetActive(value);
        }

        public void EnableRoll()
        {
            _rollButton.interactable = true;
            _rollButton.gameObject.SetActive(true);
        }
        
        public void DisableRoll()
        {
            _rollButton.interactable = false;
            _rollButton.gameObject.SetActive(false);
        }

        public void EnableEndTurn()
        {
            _endTurnButton.interactable = true;
            _endTurnButton.gameObject.SetActive(true);
        }
        
        public void DisableEndTurn()
        {
            _endTurnButton.interactable = false;
            _endTurnButton.gameObject.SetActive(false);
        }

        public void ClickRoll()
        {
            _soundManager.PlayEffect(_soundManager.SoundList.DiceRollSound);
            OnRollButtonClicked?.Invoke();
        }
        
        public void ClickEndTurn()
        {
            _soundManager.PlayEffect(_soundManager.SoundList.DiceRollSound);
            DisableEndTurn();
            OnEndTurnButtonClicked?.Invoke();
        }

        public void SetPlayerCounters()
        {
            _playerHp.Initialize(_player.HealthComponent);
            _playerMana.Initialize(_player.ManaComponent);

            armourCounter.Construct(_player.ArmorComponent);
            armourCounter.Enable();
            
            damageCounter.Construct(_player.DamageComponent);
            damageCounter.Enable();
        }

        public void OpenPausePopup() => _popupManager.OpenPopup(EnumPopup.Pause);

        public void ShowDiceTooltip(Edge topEdge, Vector3 position)
        {
            if (topEdge.edgePattern.edgeDescription == "") return;
            
            Vector3 tooltipPostion = _battleCanvas.transform.InverseTransformPoint(position);
            
            _diceTooltip.transform.localPosition = new Vector3(tooltipPostion.x,tooltipPostion.y, _diceTooltip.transform.localPosition.z);

            _diceTooltip.SetFont(_fontSettings.GetFontSetting(localization.Language, TextTag.Regular));
            
            string description = topEdge.edgePattern.edgeDescription;

            string[] tempArray = description.Split(" ");
            description = "";

            int effectCount = 0;

            for (var i = 0; i < tempArray.Length; i++)
            {
                string symbol = tempArray[i];
                
                if (symbol == "{0}")
                {
                    symbol = topEdge.edgePattern._effects[effectCount].Value.ToString();
                }

                description += symbol + " ";
            }
            
            _diceTooltip.SetText(description);

            _diceTooltip.gameObject.SetActive(true);
        }
        
        public void HideDiceTooltip() => _diceTooltip.gameObject.SetActive(false);

        public void SetDices(List<Dice.Dice> diceList)
        {
            _diceHolders = new List<DiceHolder>();

            diceList.ForEach(dice => _diceHolders.Add(dice.diceHolder));

            for (int i = 0; i < _diceHolders.Count; i++)
            {
                _diceHolders[i].OnHoldFinished += _diceCells[i].EnableLockEffect;
                _diceHolders[i].OnUnHoldFinished += _diceCells[i].DisableLockEffect;
            }
        }

        public void SetInteractableButtons(bool value)
        {
            _rollButton.interactable = value;
            _pauseButton.interactable = value;
        }
    }
}