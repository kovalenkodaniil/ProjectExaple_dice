using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleDebug : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Text lb;

    private void Awake()
    {
        slider.minValue = 1;
        slider.maxValue = 5;
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float value)
    {
        Time.timeScale = value;
        lb.text = $"Time scale: <color=#4f75cd>{value:F2}</color>";
    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnSliderValueChanged);
    }
}
