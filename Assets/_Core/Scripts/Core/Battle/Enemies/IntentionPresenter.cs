using Core.Data;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public class IntentionPresenter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image imgIcon;
        [SerializeField] private Image imgBorder;
        [SerializeField] private GameObject tooltip;
        [SerializeField] private TMP_Text lbForce;
        [SerializeField] private TMP_Text lbTooltip;
    
        public string Force { set => lbForce.text = value; }
        public string Tooltip { set => lbTooltip.text = value; }
        public Sprite Icon { set => imgIcon.sprite = value; }

        private Material materialBase;
        private Material blinkMaterial;
        private Tween tweenBlink;

        private void Awake()
        {
            SetActiveTooltip(false);

            materialBase = imgIcon.material;
        }

        public void EnableBlinkEffect(bool isEnable)
        {
            if (!isEnable)
            {
                imgIcon.material = materialBase;
                imgBorder.material = materialBase;
                if(tweenBlink is {active: true})
                    tweenBlink.Kill();
                return;
            }

            if (tweenBlink is { active: true })
                return;
        
            if (blinkMaterial == null)
                blinkMaterial = new Material(StaticDataProvider.Get<MaterialDataProvider>().Asset.blink);
        
            imgIcon.material = blinkMaterial;
            imgBorder.material = blinkMaterial;
        
            blinkMaterial.SetFloat("_BlinkIntensity", 0f);
            tweenBlink = blinkMaterial.DOFloat(0.6f, "_BlinkIntensity", 0.6f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        public void SetActiveTooltip(bool value)
        {
            tooltip.SetActive(value);
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            SetActiveTooltip(true);       
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetActiveTooltip(false);
        }
    }
}