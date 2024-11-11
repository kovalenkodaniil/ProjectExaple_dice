using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BlinkTest : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void Start()
    {
        _image.material.SetFloat( "_BlinkIntensity", 0);
    }

    public void StartBlinking()
    {
        _image.material.DOFloat(1f, "_BlinkIntensity", 2f).SetLoops(-1, LoopType.Yoyo);
    }
}
