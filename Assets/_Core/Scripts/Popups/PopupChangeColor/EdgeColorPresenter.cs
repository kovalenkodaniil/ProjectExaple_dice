using System;
using _Core.Scripts.Core.Battle.Dice;
using Object = UnityEngine.Object;

namespace Popups
{
    public class EdgeColorPresenter
    {
        public event Action<EdgeColor> OnColorSelected;
        
        private EdgeColorView _view;
        private EdgeColor _color;

        public EdgeColorPresenter(EdgeColorView view, EdgeColor color)
        {
            _view = view;
            _color = color;
        }

        public void Enable()
        {
            _view.OnClicked += SelectColor;

            _view.IconColor = _color.color;
        }
        
        public void Disable()
        {
            _view.OnClicked -= SelectColor;
            
            Object.Destroy(_view.gameObject);
        }

        public void SelectColor()
        {
            OnColorSelected?.Invoke(_color);
        }
    }
}