using System;
using System.Collections.Generic;
using Core.Data;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _Core.Scripts.Core.Battle.Combinations
{
    public class CombinationResoverView : MonoBehaviour
    {
        [field: SerializeField] public Transform StartEffectPosition { get; private set; }

        [SerializeField] private GameObject _combinationScreen;
        [SerializeField] private CombinationView combinationViewPrefab;
        [SerializeField] private Transform _previewParent;
        [SerializeField] private GameObject _animationPanel;
        [FormerlySerializedAs("imgExclamation")] [SerializeField] private GameObject goExclamation;
        [SerializeField] private List<Image> _edgeIcons;
        [SerializeField] private List<Image> _combinationIcons;
        [SerializeField] private List<CombinationView> _views;

        public void PlayEffect(CombinationConfig combination, Action callback)
        {
            SetEdges(combination);
            _animationPanel.SetActive(true);
            
            Sequence animationOrder = DOTween.Sequence();

            _edgeIcons.ForEach(edge =>
            {
                animationOrder.Append(edge.transform.DOScale(1.2f, 0.4f));
            });
            
            animationOrder.Append(_edgeIcons[0].transform.DOScale(0f, 0.4f));
            animationOrder.Join(_edgeIcons[1].transform.DOScale(0f, 0.4f));
            animationOrder.Join(_edgeIcons[2].transform.DOScale(0f, 0.4f));
            

            animationOrder.OnComplete(() =>
            {
                callback?.Invoke();
                _animationPanel.SetActive(false);
            });
        }

        public void OpenScreen()
        {
            bool value = !_combinationScreen.activeSelf;
            _combinationScreen.SetActive(value);
            goExclamation.SetActive(!value);
        }

        public void ClearAll()
        {
            _views?.ForEach(view =>
            {
                if (view != null)
                    Destroy(view.gameObject);
            });
        }

        public CombinationView CreatePreview(CombinationConfig combinationConfig)
        {
            CombinationView view = Instantiate(combinationViewPrefab, _previewParent);
            view.SetCombination(combinationConfig);

            _views.Add(view);
            
            return view;
        }

        private void SetEdges(CombinationConfig combination)
        {
            for (int i = 0; i < _combinationIcons.Count; i++)
            {
                _combinationIcons[i].color = combination.comboSequence[i].color;
                
                _edgeIcons[i].transform.localScale = Vector3.zero;
            }
        }
    }
}