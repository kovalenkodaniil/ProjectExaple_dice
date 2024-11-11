using Core.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class BuyButtonView : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMP_Text _buttonText;
        [SerializeField] private GameObject _costState;
        [SerializeField] private GameObject _maxLevelState;

        public void SetCost(int cost)
        {
            _maxLevelState.SetActive(false);
            _costState.SetActive(true);
            
            _buttonText.text = $"{cost.ToString()}";
        }

        public void SetMaxLevel()
        {
            _maxLevelState.SetActive(true);
            _costState.SetActive(false);
        }

        public void SetInteractable(bool isInteractable) => _button.interactable = isInteractable;
    }
}