using DG.Tweening;
using UnityEngine;

namespace Core.InventoryScripts
{
    public class LevelPanelComponent : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        
        public void Hide()
        {
            if (gameObject.activeSelf) _canvasGroup.DOFade(0, 0.3f).SetLink(gameObject);
        }

        public void Show()
        {
            if (gameObject.activeSelf) _canvasGroup.DOFade(1, 0.3f).SetLink(gameObject);
        }
    }
}