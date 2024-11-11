using UnityEngine;
using UnityEngine.UI;

namespace _Core.Scripts.Core.Greyness
{
    public class GreynessView : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private Button _palleteButton;

        public float CurrentValue { set => _slider.value = value; }
        
        public float MaxValue { set => _slider.maxValue = value; }

        public Button Pallete => _palleteButton;
    }
}