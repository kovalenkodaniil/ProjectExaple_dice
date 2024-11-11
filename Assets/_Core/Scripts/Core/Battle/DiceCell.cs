using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace _Core.Scripts.Core.Battle
{
    public class DiceCell : MonoBehaviour
    {
        [SerializeField] private Image _lockBorder;
        [SerializeField] private Image _lockIcon;

        public void EnableLockEffect()
        {
            _lockBorder.DOFade(1, 0.3f).SetLink(gameObject);
            _lockIcon.DOFade(1, 0.3f).SetLink(gameObject);
            _lockIcon.transform.DOScale(1, 0.3f).SetLink(gameObject);
        }

        public void DisableLockEffect()
        {
            _lockBorder.DOFade(0, 0.3f).SetLink(gameObject);
            _lockIcon.DOFade(0, 0.3f).SetLink(gameObject);
            _lockIcon.transform.DOScale(0, 0.3f).SetLink(gameObject);
        }
    }
}