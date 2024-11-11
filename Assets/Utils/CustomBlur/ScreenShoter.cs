using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class ScreenShoter : Singleton<ScreenShoter>
{
    private int _divisionSize = 1;
    private RenderTexture _renderTexture;
    private Camera _camera;

    private Camera Camera => _camera != null ? _camera : _camera = GetComponent<Camera>();

    private RenderTexture CacheRenderTexture(int divisionSize)
    {
        if (_renderTexture == null)
        {
            _divisionSize = divisionSize;
            _renderTexture = CreateRenderTexture(divisionSize);
            return _renderTexture;
        }

        if (_divisionSize == divisionSize) 
            return _renderTexture;

        _divisionSize = divisionSize;
        ReleaseRenderTexture(_renderTexture);
        _renderTexture = CreateRenderTexture(divisionSize);

        return _renderTexture;
    }

    private Vector2Int GetSize(int divisionSize)
    {
        var screenSize = new Vector2Int(Screen.width, Screen.height);
        var acpect = screenSize.x * 1f / Screen.width;
        var wight = screenSize.x * acpect / divisionSize;
        var height = screenSize.y * acpect / divisionSize;

        return new Vector2Int((int)wight, (int)height);
    }

    protected override void AwakeInner()
    {
        if (_renderTexture == null) 
            _renderTexture = CreateRenderTexture(_divisionSize);
    }

    public void SpriteCamera(int divisionSize, UnityAction<Sprite> callback)
    {
        IETextureCamera(divisionSize, x => callback.Invoke(x.CreateSprite()));
    }

    public IEnumerator IESpriteCamera(int divisionSize, UnityAction<Sprite> callback)
    {
        IETextureCamera(divisionSize, x =>
        {
            if (x == null)
            {
                callback.Invoke(null);
                return;
            }

            callback.Invoke(x.CreateSprite());
        });
        yield break;
    }

    public void IETextureCamera(int devisionSize, UnityAction<Texture2D> callback)
    {
        var selectCamera = Camera;
        var rTexture = CacheRenderTexture(devisionSize);
        var tex = SpriteExtension.GetNewTexture(rTexture.width, rTexture.height);

        selectCamera.targetTexture = rTexture;
        selectCamera.Render();
        selectCamera.targetTexture = null;

        var activeRT = RenderTexture.active;
        RenderTexture.active = rTexture;
        tex = tex.GetReadPixels();
        RenderTexture.active = activeRT;
        tex.Apply();
        callback.Invoke(tex);
    }

    private RenderTexture CreateRenderTexture(int devisionSize)
    {
        var size = GetSize(devisionSize);
        var renderTexture = RenderTexture.GetTemporary(size.x, size.y);
        renderTexture.autoGenerateMips = false;

        return renderTexture;
    }

    private void ReleaseRenderTexture(RenderTexture rTexture) => RenderTexture.ReleaseTemporary(rTexture);
}
