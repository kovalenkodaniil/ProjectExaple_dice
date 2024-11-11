using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StatBarAnimator : MonoBehaviour
    {
        [SerializeField] private Slider _bar;
        [SerializeField] private Image _damageBar;
        [SerializeField] private Image _healBar;
        [SerializeField] private Image _manaBar;

        public void PlayDamageAnimation(int value, int lastValue)
        {
            _bar.value = value;
            _damageBar.enabled = true;

            _damageBar.fillAmount = (float)lastValue / _bar.maxValue;
            _damageBar.DOFillAmount((float)value / _bar.maxValue, 0.4f)
                .OnKill(() => _damageBar.fillAmount = value)
                .OnComplete(() => _damageBar.enabled = false);
        }
        
        public void PlayHealAnimation(int value, int lastValue)
        {
            _healBar.enabled = true;
            _healBar.fillAmount = (float)value / _bar.maxValue;
            
            _bar.DOValue(value, 0.4f).OnComplete(() => _healBar.enabled = false);
        }

        public void PlayManaIncrease(int value, int lastValue)
        {
            _manaBar.enabled = true;
            _manaBar.fillAmount = (float)value / _bar.maxValue;
            
            _bar.DOValue(value, 0.4f).OnComplete(() => _manaBar.enabled = false);
        }
        
        public void PlayManaDecrease(int value, int lastValue)
        {
            _manaBar.enabled = false;
            
            _bar.DOValue(value, 0.4f);
        }
    }
}