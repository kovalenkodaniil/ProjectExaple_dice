using _Core.Scripts.Core.Battle.Stat;
using TMPro;
using UnityEngine;

public class BattleArmourCounterPresenter : MonoBehaviour
{
    private IStatsComponent armourComponent;
    [SerializeField] private TMP_Text lbCounter;

    public void Construct(IStatsComponent armourComponent)
    {
        this.armourComponent = armourComponent;
    }

    public void Enable()
    {
        armourComponent.OnValueChanged += OnValueChanged;
        OnValueChanged(armourComponent.CurrentValue);
    }

    public void Disabe()
    {
        armourComponent.OnValueChanged -= OnValueChanged;
    }

    private void OnValueChanged(int curr)
    {
        lbCounter.text = curr.ToString();
        gameObject.SetActive(curr > 0);
    }
}