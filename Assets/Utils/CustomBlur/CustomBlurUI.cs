using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CustomBlurUI : MonoBehaviour
{
    [SerializeField] private int _radius = 1;
    [SerializeField] private int _iterations = 4;
    [SerializeField] private int _devisionSize = 7;
    [SerializeField] private bool _showOnEnable;

    private Image _image;
    private float _rSum;
    private float _gSum;
    private float _bSum;
    private int _width;
    private int _height;
    private int _windowSize;
    private Color _colorPixel = Color.white;

    private Image Image => _image != null ? _image : _image = GetComponent<Image>();

    private void OnEnable()
    {
        if (_showOnEnable)
           Show();
    }

    public void Show() => StartCoroutine(IEShow());

    private IEnumerator IEShow()
    {
        Image.enabled = false;

        var blurCamera = ScreenShoter.Instance;
        Texture2D texture = null;

        yield return blurCamera.IESpriteCamera(_devisionSize, x =>
        {
            Image.sprite = x;
            Image.enabled = true;
            texture = x.texture;
        });
        yield return null;
        _width = texture.width;
        _height = texture.height;
        _windowSize = _radius * 2 + 1;
        yield return IEBlurCalculation(texture, _iterations, _radius);
    }

    private IEnumerator IEBlurCalculation(Texture2D texture, int iterations, int radius)
    {
        var colors = texture.GetPixels();
        for (var i = 0; i < iterations; i++)
        {
            var taskHorizontal = GetBlurHorizontal(colors, radius);
            yield return taskHorizontal;
            var taskVertical = GetBlurVertical(taskHorizontal.Result, radius);
            yield return taskVertical;
            colors = taskVertical.Result;
            texture.SetPixels(colors);
            texture.Apply();
            yield return null;
        }
    }

    private async Task<Color[]> GetBlurVertical(Color[] texture, int radius)
    {
        for (var X = 0; X < _width; X++)
        {
            ResetSum();
            for (var Y = 0; Y < _height; Y++)
            {
                if (Y == 0)
                {
                    for (var y = -radius; y <= radius; y++)
                    {
                        AddPixel(GetPixelWithY(texture, X, y));
                    }
                }
                else
                {
                    SubstPixel(GetPixelWithY(texture, X, Y - radius - 1));
                    AddPixel(GetPixelWithY(texture, X, Y + radius));
                }

                texture[ToIndex(X, Y)] = GetColorPixel();
            }
        }

        await Task.Yield();
        return texture;
    }

    private async Task<Color[]> GetBlurHorizontal(Color[] texture, int radius)
    {
        for (var Y = 0; Y < _height; Y++)
        {
            ResetSum();
            for (var X = 0; X < _width; X++)
            {
                if (X == 0)
                {
                    for (var x = -radius; x <= radius; x++)
                    {
                        AddPixel(GetPixelWithX(texture, x, Y));
                    }
                }
                else
                {
                    SubstPixel(GetPixelWithX(texture, X - radius - 1, Y));
                    AddPixel(GetPixelWithX(texture, X + radius, Y));
                }
                texture[ToIndex(X, Y)] = GetColorPixel();
            }
        }

        await Task.Yield();
        return texture;
    }

    private int ToIndex(int x, int y) => y * _width + x;

    private void AddPixel(Color pixel)
    {
        _rSum += pixel.r;
        _gSum += pixel.g;
        _bSum += pixel.b;
    }

    private void SubstPixel(Color pixel)
    {
        _rSum -= pixel.r;
        _gSum -= pixel.g;
        _bSum -= pixel.b;
    }

    private void ResetSum() => _rSum = _gSum = _bSum = 0f;

    private Color GetPixelWithY(Color[] texture, int x, int y) => texture[ToIndex(x, Mathf.Clamp(y, 0, _height - 1))];

    private Color GetPixelWithX(Color[] texture, int x, int y) => texture[ToIndex(Mathf.Clamp(x, 0, _width - 1), y)];

    private Color GetColorPixel()
    {
        _colorPixel.r = _rSum / _windowSize;
        _colorPixel.g = _gSum / _windowSize;
        _colorPixel.b = _bSum / _windowSize;

        return _colorPixel;
    }
}
