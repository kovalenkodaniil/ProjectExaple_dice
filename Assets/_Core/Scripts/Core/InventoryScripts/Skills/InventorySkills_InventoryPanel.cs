using System;
using System.Collections;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.SkillScripts;
using _Core.Scripts.Core.Data;
using Core.Data;
using Core.PreBattle;
using DG.Tweening;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace Core.InventoryScripts
{
    public class InventorySkills_InventoryPanel : MonoBehaviour, ITab
    {
        [Inject] private IObjectResolver _objectResolver;
        [Inject] private GameDataConfig _gameStaticData;
        [Inject] private Managers.Localization localization;
        [Inject] private Player _player;
        
        [SerializeField] private GameObject _container;
        [SerializeField] private CanvasGroup _infoPanel;
        [SerializeField] private CanvasGroup _inBattlePanel;
        [SerializeField] private CanvasGroup _contentPanel;

        [SerializeField] private InventorySkills_SkillDetailPanelView inventorySkillsSkillDetailPanelView;
        [SerializeField] private InventorySkill_AvailableSkillPanel _availableSkillPanel;
        [SerializeField] private InventorySkills_SkillInBattlePanel _skillInBattlePanel;
        
        private SkillDataProvider _provider;
        private InventorySkills_SkillDetailPresenter _skillDetailPresenter;
        private List<InventorySpells_ItemPreviewPresenter> _spellPresenters;
        private InventorySpells_ItemPreviewPresenter current;
        private Coroutine _clickExpectantRoutine;
        
        public event Action OnOpen;
        public event Action<ITab> OnClose;

        public void Initialize(Player player)
        {
            _player = player;

            if (inventorySkillsSkillDetailPanelView == null)
                Debug.Log("inventorySkillsSkillDetailPanelView null");
            
            if (player.skillStorage == null)
                Debug.Log("player.skillStorage null");
            
            _skillDetailPresenter = new InventorySkills_SkillDetailPresenter(inventorySkillsSkillDetailPanelView, _player.skillStorage);
            
            _availableSkillPanel.Initialize(_player.skillStorage);
            _skillInBattlePanel.Initialize(_player.skillStorage);
        }

        public void Open()
        {
            Sub();
            
            _skillDetailPresenter.Enable();
            _availableSkillPanel.Enable();
            _skillInBattlePanel.Enable();

            _container.SetActive(true);

            PlayOpenAnimation();
        }

        public void Close()
        {
            _skillDetailPresenter.Disable();
            _availableSkillPanel.Disable();
            _skillInBattlePanel.Disable();
            
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
            _skillDetailPresenter.OnSkillSelected += TryReplaceSkillInBattle;
            _availableSkillPanel.OnSkillSelected += _skillDetailPresenter.ShowSkillDetail;
        }

        private void UnSub()
        {
            _skillDetailPresenter.OnSkillSelected -= TryReplaceSkillInBattle;
            _availableSkillPanel.OnSkillSelected -= _skillDetailPresenter.ShowSkillDetail;
        }

        private void TryReplaceSkillInBattle(SkillConfig config)
        {
            _clickExpectantRoutine = StartCoroutine(WaitReplacementInBattleSkill(config));
        }

        private IEnumerator WaitReplacementInBattleSkill(SkillConfig config)
        {
            _skillInBattlePanel.ShowWaitingEffect();
            
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            var mousePosition = Input.mousePosition;

            if (_skillInBattlePanel.IsClickedOnPanel(mousePosition, out InventorySkills_SkillInBattlePresenter presenter))
            {
                _player.skillStorage.ReplaceBattleSkill(presenter.CurrentSkill, config);
                presenter.ReplaceSkill(config);
                
                _availableSkillPanel.SetBattleMark();
            }
            
            _skillInBattlePanel.ShowWaitingEffect(false);
        }
    }
}