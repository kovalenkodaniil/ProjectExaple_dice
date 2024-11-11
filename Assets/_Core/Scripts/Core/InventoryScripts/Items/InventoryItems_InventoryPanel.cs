using System;
using System.Collections;
using _Core.Scripts.Core.Data;
using Core.Data;
using Core.Items;
using Core.PreBattle;
using DG.Tweening;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace Core.InventoryScripts.Items
{
    public class InventoryItems_InventoryPanel : MonoBehaviour, ITab
    {
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private GameDataConfig _gameStaticData;
        [Inject] private Managers.Localization localization;
        [Inject] private Player _player;
        
        [SerializeField] private GameObject _container;
        [SerializeField] private CanvasGroup _infoPanel;
        [SerializeField] private CanvasGroup _inBattlePanel;
        [SerializeField] private CanvasGroup _contentPanel;

        [SerializeField] private InventoryItems_ItemDetailPanelView _itemDetailPanelView;
        [SerializeField] private InventoryItems_AvailableItemPanel _availableItemPanel;
        [SerializeField] private InventoryItems_ItemInBattlePanel _itemInBattlePanel;
        
        private InventoryItems_ItemDetailPresenter _skillDetailPresenter;
        private Coroutine _clickExpectantRoutine;
        
        public event Action OnOpen;
        public event Action<ITab> OnClose;

        public void Initialize(Player player)
        {
            _player = player;

            _skillDetailPresenter = new InventoryItems_ItemDetailPresenter(_itemDetailPanelView, _player.itemStorage);
            _availableItemPanel.Initialize(_player.itemStorage);
            _itemInBattlePanel.Initialize(_player.itemStorage);
        }

        public void Open()
        {
            Sub();
            
            _skillDetailPresenter.Enable();
            _availableItemPanel.Enable();
            _itemInBattlePanel.Enable();

            _container.SetActive(true);

            PlayOpenAnimation();
        }

        public void Close()
        {
            _skillDetailPresenter.Disable();
            _availableItemPanel.Disable();
            _itemInBattlePanel.Disable();
            
            UnSub();
            
            PlayCloseAnimation(() =>
            {
                _container.SetActive(false);
                OnClose?.Invoke(this);
            });
            
            if (_clickExpectantRoutine != null)
                StopCoroutine(_clickExpectantRoutine);
        }
        
        public void PlayOpenAnimation()
        {
            _infoPanel.DOFade(1, 0.4f);
            _contentPanel.DOFade(1, 0.4f);
            _inBattlePanel.DOFade(1, 0.4f);
        }

        public void PlayCloseAnimation(Action callback)
        {
            _infoPanel.DOFade(0, 0.4f);
            _contentPanel.DOFade(0, 0.4f);
            _inBattlePanel.DOFade(0, 0.4f).OnComplete(() => callback?.Invoke());
        }

        private void Sub()
        {
            _skillDetailPresenter.OnItemSelected += TryReplaceItemInBattle;
            _availableItemPanel.OnItemSelected += _skillDetailPresenter.ShowSkillDetail;
        }

        private void UnSub()
        {
            _skillDetailPresenter.OnItemSelected -= TryReplaceItemInBattle;
            _availableItemPanel.OnItemSelected -= _skillDetailPresenter.ShowSkillDetail;
        }

        private void TryReplaceItemInBattle(ItemConfig config)
        {
            _clickExpectantRoutine = StartCoroutine(WaitReplacementInBattleSkill(config));
        }

        private IEnumerator WaitReplacementInBattleSkill(ItemConfig config)
        {
            _itemInBattlePanel.ShowWaitingEffect();
            
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            var mousePosition = Input.mousePosition;

            if (_itemInBattlePanel.IsClickedOnPanel(mousePosition, out InventoryItems_ItemInBattlePresenter presenter))
            {
                _player.itemStorage.ReplaceItemInBattle(presenter.CurrentItem, config);
                presenter.ReplaceItem(config);
                
                _availableItemPanel.SetBattleMark();
            }
            
            _itemInBattlePanel.ShowWaitingEffect(false);
        }
    }
}