using System;
using System.Collections;
using Core.Data;
using Core.PreBattle;
using DG.Tweening;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace Core.InventoryScripts
{
    public class DiceInventoryPanel : MonoBehaviour, ITab
    {
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private Player _player;
        
        public event Action OnOpen;
        public event Action<ITab> OnClose;

        private DiceInfoPresenter _diceInfoPresenter;
        private DiceData _selectedDice;
        private Coroutine _clickExpectantRoutine;

        [SerializeField] private AvailableDicePanel _availableDicePanel;
        [SerializeField] private DiceInBattlePanel _diceInBattlePanel;
        [SerializeField] private GameObject _container;
        [SerializeField] private DiceInventoryView _diceInventoryView;
        [SerializeField] private InventorClickHolder _clickHolder;
        [SerializeField] private UpgradeSelector _upgradePopup;
        [SerializeField] private CanvasGroup _infoPanel;
        [SerializeField] private CanvasGroup _inBattlePanel;
        [SerializeField] private CanvasGroup _contentPanel;

        public void Initialize(Player player)
        {
            _objectResolver.Inject(_upgradePopup);

            _availableDicePanel.Initialize(_player);
            _diceInBattlePanel.Initialize(_player, _clickHolder);
            
            _diceInfoPresenter = new DiceInfoPresenter(_diceInventoryView, _player);
        }

        public void Open()
        {
            SubEvent();
            
            _availableDicePanel.Open();
            _diceInBattlePanel.Show();
            _diceInfoPresenter.Enable();
            
            _container.gameObject.SetActive(true);
            
            PlayOpenAnimation();
            
            _clickHolder.OnClicked += Clicked;
        }

        public void Close()
        {
            UnSubEvent();
            DisableDiceInBattlePanel();
            
            _availableDicePanel.Close();
            _diceInBattlePanel.Hide();
            _diceInfoPresenter.Disable();

            _clickHolder.OnClicked -= Clicked;

            PlayCloseAnimation(() =>
            {
                _container.gameObject.SetActive(false);
                OnClose?.Invoke(this);
            });
        }

        private void PlayOpenAnimation()
        {
            _infoPanel.DOFade(1, 0.4f);
            _contentPanel.DOFade(1, 0.4f);
            _inBattlePanel.DOFade(1, 0.4f);
        }

        private void PlayCloseAnimation(Action callback)
        {
            _diceInventoryView.PlayFadeAnimation();
            
            _infoPanel.DOFade(0, 0.4f);
            _contentPanel.DOFade(0, 0.4f);
            _inBattlePanel.DOFade(0, 0.4f).OnComplete(() =>
            {
                callback?.Invoke();
            });
        }

        private void SubEvent()
        {
            _availableDicePanel.OnDiceSelected += _diceInfoPresenter.ShowDiceInfo;
            _diceInfoPresenter.OnDiceSelectedForBattle += AddDiceInBattle;
            _diceInfoPresenter.OnDiceTryUpgraded += _upgradePopup.Open;
        }

        private void UnSubEvent()
        {
            _availableDicePanel.OnDiceSelected -= _diceInfoPresenter.ShowDiceInfo;
            _diceInfoPresenter.OnDiceSelectedForBattle -= AddDiceInBattle;
            _diceInfoPresenter.OnDiceTryUpgraded -= _upgradePopup.Open;
        }

        private void AddDiceInBattle(DiceData diceData)
        {
            _diceInBattlePanel.EnableSelectingMode(diceData);
        }

        private void DisableDiceInBattlePanel()
        {
            if (_clickExpectantRoutine != null)
                StopCoroutine(_clickExpectantRoutine);
            
            _diceInBattlePanel.DisableSelectingMode();
        }

        private void Clicked()
        {
            _clickHolder.gameObject.SetActive(false);
            
            _clickExpectantRoutine = StartCoroutine(WaitClickEvent());
        }

        private IEnumerator WaitClickEvent()
        {
            yield return new WaitForSeconds(0.1f);

            DisableDiceInBattlePanel();
        }
    }
}