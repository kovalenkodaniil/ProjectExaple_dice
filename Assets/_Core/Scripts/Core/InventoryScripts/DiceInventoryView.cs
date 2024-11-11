using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Coffee.UIExtensions;
using Core.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class DiceInventoryView : MonoBehaviour
    {
        public event Action OnUpgradeClicked;
        public event Action OnDiceSelected;
        
        [SerializeField] private TMP_Text _diceName;
        [SerializeField] private TMP_Text _diceLevel;
        [SerializeField] private TMP_Text _quantity;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private List<EdgePreview> _currentEdges;
        [SerializeField] private BuyButtonView _upgradeButton;
        [SerializeField] private Button _selectButton;
        [SerializeField] private DiceUIPreview _dicePreview;
        [SerializeField] private CanvasGroup _diceContent;
        [SerializeField] private UIParticle _upgradeEffect;


        public void PlayAppearanceContent()
        {
            _diceContent.DOFade(1, 0.4f);
        }

        public void PlayHideContent(Action callback)
        {
            _diceContent.DOFade(0, 0.4f).OnComplete(() => callback?.Invoke());
            _dicePreview.PlayHideAnimation();
        }
        
        public void SetName(string name) => _diceName.text = name;

        public void SetDicePreview(DiceConfig diceConfig) => _dicePreview.InitializeInventory(diceConfig, 97);
        
        public void PlayFadeAnimation(Action callback = null) => _dicePreview.PlayHideAnimation(callback);

        public void PlayUpgradeAnimation(int upgradeIndex, Edge upgradeEdge)
        {
            _upgradeEffect.transform.position = _currentEdges[upgradeIndex].transform.position;
            
            
            _currentEdges[upgradeIndex].PlayHideAnimation(() =>
            {
                _currentEdges[upgradeIndex].SetEdge(upgradeEdge.edgePattern.edgeIcon, GetEdgeDescription(upgradeEdge));
                
                if (upgradeEdge.edgePattern.colors.Length > 0)
                    _currentEdges[upgradeIndex].SetColor(upgradeEdge.edgePattern.colors);
                
                _currentEdges[upgradeIndex].PlayUpgradeAnimation();
                
                _upgradeEffect.Play();
            });
        }

        public void PlayChangeLevelAnimation(string diceLevel)
        {
            Sequence animationOrder = DOTween.Sequence();

            animationOrder.Append(_diceLevel.DOFade(0, 0.2f));
            animationOrder.Join(_diceLevel.transform.DOScale(0, 0.2f).OnComplete(() => SetLevel(diceLevel)));
            animationOrder.Append(_diceLevel.DOFade(1, 0.2f));
            animationOrder.Join(_diceLevel.transform.DOScale(1, 0.2f));
        }

        public void SetQuantity(int quantity) => _quantity.text = quantity.ToString();
        
        public void SetDescription(string description) => _description.text = description;

        public void DisableUpgradeButton()
        {
            _upgradeButton.SetInteractable(false);
        }
        
        public void SetMaxLevelUpgradeButton()
        {
            _upgradeButton.SetInteractable(false);
            _upgradeButton.SetMaxLevel();
        }

        public void SetEnableSelectButton(bool isEnable) => _selectButton.interactable = isEnable;

        public void SetLevel(string level) => _diceLevel.text = level;

        public void SetUpgradeCost(int cost)
        {
            _upgradeButton.SetInteractable(true);
            _upgradeButton.SetCost(cost);
        }

        public void SetCurrentEdges(List<Edge> edges)
        {
            for (int i = 0; i < _currentEdges.Count; i++)
            {
                _currentEdges[i].SetEdge(edges[i].edgePattern.edgeIcon, GetEdgeDescription(edges[i]));
                
                if (edges[i].edgePattern.colors.Length > 0)
                    _currentEdges[i].SetColor(edges[i].edgePattern.colors);
            }
        }

        public void Click() => OnUpgradeClicked?.Invoke();

        public void SelectDice() => OnDiceSelected?.Invoke();

        private string GetEdgeDescription(Edge edge)
        {
            if (edge.edgePattern.edgeDescription == "") return "";
            
            string description = edge.edgePattern.edgeDescription;

            string[] tempArray = description.Split(" ");
            description = "";

            int effectCount = 0;

            for (var i = 0; i < tempArray.Length; i++)
            {
                string symbol = tempArray[i];
                
                if (symbol == "{0}")
                {
                    symbol = edge.edgePattern._effects[effectCount].Value.ToString();
                }

                description += symbol + " ";
            }

            return description;
        }
    }
}