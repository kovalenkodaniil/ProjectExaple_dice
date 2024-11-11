using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using Core.Localization;
using Localization;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class DiceUpgradeView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image _panel;
        [SerializeField] private Sprite _defaultPanelSprite;
        [SerializeField] private Sprite _selectedPanelSprite;
        [SerializeField] private Sprite _highlightedPanelSprite;
        [SerializeField] private DiceUIPreview _dice;
        [SerializeField] private TMP_Text _diceName;
        [SerializeField] private GameObject corners;
        [SerializeField] private List<UpgradeEdgePreview> _edges;
        
        private DiceConfig _diceConfig;
        private bool _isSelected;
        
        public event Action<DiceUpgradeView, DiceConfig> OnSelected;

        private void Awake()
        {
            corners.gameObject.SetActive(false);
        }

        public void SetDice(DiceConfig diceConfig)
        {
            _diceConfig = diceConfig;
            _dice.InitializeInventory(diceConfig, 55);
        }

        public void SetDiceName(string name) => _diceName.text = name;

        public void Deselect()
        {
            _isSelected = false;
            _panel.sprite = _defaultPanelSprite;
            corners.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _panel.sprite = _selectedPanelSprite;
            _isSelected = true;

            _edges.ForEach(edge => edge.EnableSelectedMode(true));
            
            OnSelected?.Invoke(this, _diceConfig);
        }

        public void CreateTooltip()
        {
            for (int i = 0; i < _diceConfig.Edges.Count; i++)
            {
                _edges[i].SetEdge(_diceConfig.Edges[i].edgePattern.edgeIcon, GetEdgeDescription(_diceConfig.Edges[i]));
                
                _edges[i].EnableSelectedMode(false);
            }
        }

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

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;
            
            corners.gameObject.SetActive(true);
            _panel.sprite = _highlightedPanelSprite;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;

            Deselect();
        }
    }
}