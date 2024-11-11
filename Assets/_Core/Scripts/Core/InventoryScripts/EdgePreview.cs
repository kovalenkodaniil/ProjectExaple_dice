using System;
using _Core.Scripts.Core.Battle.Dice;
using Core.Data;
using DG.Tweening;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.InventoryScripts
{
    public class EdgePreview : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TooltipWindow _tooltip;
        [SerializeField] protected Image _edgeIcon;

        public void SetEdge(Sprite edgeSprite, string description)
        {
            _tooltip.SetText(description);

            _edgeIcon.gameObject.SetActive(edgeSprite != null);
            _edgeIcon.sprite = edgeSprite;

            _tooltip.gameObject.SetActive(false);
        }

        public void SetColor(EdgeColor[] colors)
        {
            if (_edgeIcon.sprite == null) return;
            
            _edgeIcon.material = new Material(StaticDataProvider.Get<MaterialDataProvider>().Asset.twoColors);
            _edgeIcon.material.SetTexture("_MainTex", _edgeIcon.sprite.texture);
            _edgeIcon.material.SetColor("_FirstColor", colors[0].color);
            
            if (colors.Length > 1)
                _edgeIcon.material.SetColor("_SecondColor", colors[1].color);
            else
                _edgeIcon.material.SetColor("_SecondColor", colors[0].color);
        }

        public void PlayHideAnimation(Action callback)
        {
            _edgeIcon.transform.DOScale(0.6f, 0.3f);
            _edgeIcon.DOFade(0, 0.3f).OnComplete(() => callback?.Invoke());
        }

        public void PlayUpgradeAnimation()
        {
            _edgeIcon.transform.localScale = new Vector3(2, 2, 2);
            
            _edgeIcon.DOFade(1, 0.2f);
            _edgeIcon.transform.DOScale(0.8f, 0.4f).SetDelay(0.2f);
        }

        public void OnPointerEnter(PointerEventData eventData) => _tooltip.gameObject.SetActive(true);

        public void OnPointerExit(PointerEventData eventData) => _tooltip.gameObject.SetActive(false);
    }
}