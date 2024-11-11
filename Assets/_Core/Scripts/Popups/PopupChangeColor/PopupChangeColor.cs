using System;
using System.Collections.Generic;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using UnityEngine;

namespace Popups
{
    public class PopupChangeColor : MonoBehaviour
    {
        private EdgeColorDataProvider _provider;
        private List<EdgeColorPresenter> _presenters;
        private Dice _currentDice;
        
        [SerializeField] private GameObject _container;
        [SerializeField] private EdgeColorView _viewPrefab;
        [SerializeField] private Transform _colorTransform;

        public void Open(Dice dice)
        {
            _provider = StaticDataProvider.Get<EdgeColorDataProvider>();
            _currentDice = dice;
            
            _container.gameObject.SetActive(true);
            
            CreateColors();
        }

        public void Close()
        {
            _presenters.ForEach(presenter =>
            {
                presenter.OnColorSelected -= ChangeDiceColor;
                presenter.Disable();
            });
            
            _container.gameObject.SetActive(false);
        }

        private void CreateColors()
        {
            _presenters = new List<EdgeColorPresenter>();
            
            _provider.colors.ForEach(color =>
            {
                EdgeColorView view = Instantiate(_viewPrefab, _colorTransform);

                EdgeColorPresenter presenter = new EdgeColorPresenter(view, color);

                _presenters.Add(presenter);
                
                presenter.Enable();
                presenter.OnColorSelected += ChangeDiceColor;
            });
        }

        private void ChangeDiceColor(EdgeColor color)
        {
            _currentDice.ChangeColor(color);
            
            Close();
        }
    }
}