using System;
using System.Collections.Generic;
using Core.Data;
using DG.Tweening;
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Core.Scripts.Core.Battle.Enemies
{
    public abstract class EnemyBase : MonoBehaviour
    {
        private Stack<IntentionPresenter> intentionPool = new(3);
        private List<IntentionPresenter> currentIntentions = new(3);
        
        [SerializeField] protected Image img;
        [SerializeField] protected Slider healthBar;
        [SerializeField] protected Outline outline;
        [SerializeField] protected IntentionPresenter intentionTemplate;
        [SerializeField] protected Transform intentionContent;
        [SerializeField] protected RectTransform colliderRect;
        [SerializeField] protected TMP_Text tmpHealth;
        [SerializeField] protected TMP_Text tmpArmor;
        [SerializeField] protected Animation animArmor;
        [SerializeField] protected GameObject armorCounter;
        
        public Sprite Image { set => img.sprite = value; }
        public string HealthText { set => tmpHealth.text = value; }
        public string ArmorText { set => tmpArmor.text = value; }
        public RectTransform RectTransform => transform as RectTransform;
        public RectTransform RectCollider => img.transform as RectTransform;

        private Tween tweenOutline; 
        private Material baseMat;
        private Material outlineMat;

        private void Start()
        {
            intentionTemplate.gameObject.SetActive(false);
            outline.enabled = false;

            baseMat = img.material;
            outlineMat = new Material(StaticDataProvider.Get<MaterialDataProvider>().Asset.outline);
            outlineMat.SetColor("Mask Color", outline.effectColor);
        }
        

        public void SetActiveIntentions(bool value)
        {
            intentionContent.gameObject.SetActive(value);
        }

        public void ShowIntentions(Queue<EnemyTurn> modelCurrentTurnIntentions)
        {
            foreach (var intention in modelCurrentTurnIntentions)
                CreateIntention(intention);
            
            SetActiveIntentions(true);
        }

        public void HideFirstIntention()
        {
            var intention = currentIntentions[0];
            currentIntentions.Remove(intention);
            intention.gameObject.SetActive(false);   
            intentionPool.Push(intention);
        }

        public void EnableIntentionsBlinking(bool isBlinking) => currentIntentions.ForEach(intention => intention.EnableBlinkEffect(isBlinking));

        public void HideIntention(int index)
        {
            currentIntentions[index].gameObject.SetActive(false);
        }

        public bool IsMouseOverEnemy(Vector2 mousePosition)
        {
            RectTransform rectTransform = img.rectTransform;

            bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(RectCollider, mousePosition, GlobalCamera.Camera, out Vector2 localMousePosition);
        
            if (!isInside) return false;

            Sprite sprite = img.sprite;
            Rect spriteRect = sprite.rect;
        
            float x = (localMousePosition.x + rectTransform.rect.width * rectTransform.pivot.x) / rectTransform.rect.width;
            float y = (localMousePosition.y + rectTransform.rect.height * rectTransform.pivot.y) / rectTransform.rect.height;
        
            x = (spriteRect.x + x * spriteRect.width) / sprite.texture.width;
            y = (spriteRect.y + y * spriteRect.height) / sprite.texture.height;

            Color pixelColor = sprite.texture.GetPixelBilinear(x, y);

            return pixelColor.a > 0.1f;
        }

        public bool IsMouseOverIntention(Vector3 mousePos, out int intentionIndex)
        {
            intentionIndex = 0;
            
            for (int i = 0; i < currentIntentions.Count; i++)
            {
                var tfm = currentIntentions[i].transform as RectTransform;
                var localMousePosition = tfm.InverseTransformPoint(mousePos);

                if (tfm.rect.Contains(localMousePosition))
                {
                    intentionIndex = i;
                    return true;
                }
            }

            return false;
        }

        public void PlayOutline(bool enable)
        {
            outline.enabled = enable;
            
            if (!enable)
            {
                img.material = baseMat;
                if(tweenOutline is {active: true})
                    tweenOutline.Kill();
                return;
            }

            if (tweenOutline is { active: true })
                return;

            img.material = outlineMat;
            outlineMat.SetFloat("_GlowIntensity", 0.5f);
            tweenOutline = outlineMat.DOFloat(3.5f, "_GlowIntensity", 0.5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        public void SetHealth(float value, float maxValue)
        {
            healthBar.value = value;
            healthBar.maxValue = maxValue;
        }
        
        public void SetArmor(int value)
        {
            armorCounter.SetActive(value > 0);
            animArmor.Play();
        }

        private void CreateIntention(EnemyTurn turn)
        {
             var widget = intentionPool.Count == 0 
                ? Instantiate(intentionTemplate, intentionContent) 
                : intentionPool.Pop();

            widget.Icon = turn.action.icon;
            widget.Force = turn.power.ToString();

            var effectTranslate = Managers.Localization.GetActionTranslate(Enum.GetName(typeof(EnumEffects), turn.action.effects[0]));
            widget.Tooltip = $"{effectTranslate} {turn.power}";
            widget.gameObject.SetActive(true);
            currentIntentions.Add(widget);
        }
    }
}