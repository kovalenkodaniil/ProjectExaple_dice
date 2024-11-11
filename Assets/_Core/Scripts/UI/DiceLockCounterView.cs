using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DiceLockCounterView : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Color _blockColor;

        public void SetBlockStatus(bool isBlocking)
        {
            if (isBlocking)
                _icon.DOColor(_blockColor, 0.2f);
            else
                _icon.DOColor(Color.white, 0.2f);
        }
    }
}