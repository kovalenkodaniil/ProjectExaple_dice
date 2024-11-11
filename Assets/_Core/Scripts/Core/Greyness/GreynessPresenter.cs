using Core.Data.Consumable;
using PlayerScripts;
using UnityEngine;
using VContainer;

namespace _Core.Scripts.Core.Greyness
{
    public class GreynessPresenter : MonoBehaviour
    {
        [Inject] private GreynessManager _greynessManager;
        [Inject] private Player _player;
        
        [SerializeField] private GreynessView _view;

        public void Initialize()
        {
            UpdateView(_greynessManager.CurrentStage);
            Sub();
        }

        public void OnDisable()
        {
            Unsub();
        }

        public void UsePalette()
        {
            if (_player.consumableStorage.TrySpend(EnumConsumable.Palette))
                _greynessManager.data.Stage -= 2;
        }

        private void Sub()
        {
            _greynessManager.data.OnValueChanged += UpdateView;
        }

        private void Unsub()
        {
            _greynessManager.data.OnValueChanged -= UpdateView;
        }

        private void UpdateView(int currentStage)
        {
            _view.MaxValue = _greynessManager.MaxStage;
            _view.CurrentValue = currentStage;

            _view.Pallete.gameObject.SetActive(_player.consumableStorage.Contains(EnumConsumable.Palette));
        }
    }
}