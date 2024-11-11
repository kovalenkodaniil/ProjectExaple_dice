using System;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor.UI;
using UnityEditor;
#endif

public class ButtonPressEffect : Button
{
    private Action currentOnPointerDown;
    private Action currentOnPointerUp;

    private Vector3 scaleBase;
    private Tween tween;
    
    public enum EnumEffect { Shift }

    [SerializeField] private EnumEffect effect;

    protected override void OnEnable()
    {
        base.OnEnable();

        scaleBase = targetGraphic.rectTransform.localScale;

        switch (effect)
        {
            case EnumEffect.Shift:
                currentOnPointerUp = Shift_OnPointerUp;
                currentOnPointerDown = Shift_OnPointerDown;
                break;
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        currentOnPointerDown?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        currentOnPointerUp?.Invoke();
    }

    private void Shift_OnPointerUp()
    {
        if (tween is { active: true })
            tween?.Kill();
        
        tween = targetGraphic.rectTransform
            .DOScale(scaleBase, 0.1f)
            .SetLink(gameObject);
    }
    
    private void Shift_OnPointerDown()
    {
        if (tween is { active: true })
            tween?.Kill();
        
        tween = targetGraphic.rectTransform
            .DOScale(-0.1f, 0.1f)
            .SetRelative()
            .SetLink(gameObject);
            
        SoundManager.Instance.PlayEffect(SoundManager.Instance.SoundList.ButtonClick);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ButtonPressEffect))]
public class ButtonPressEffectEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(serializedObject.FindProperty("effect"));
        EditorGUILayout.Space(10);
        base.OnInspectorGUI();
    }
}
#endif