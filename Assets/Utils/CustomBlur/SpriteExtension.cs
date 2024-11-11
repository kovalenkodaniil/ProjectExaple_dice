using UnityEngine;

public static class SpriteExtension
{
    public static Sprite CreateSprite(this Texture2D result)
    {
        var rect = new Rect(0, 0, result.width, result.height);
        var pivot = Vector2.one * .5f;
        var sprite = Sprite.Create(result, rect, pivot);
        sprite.name = result.name;
        return sprite;
    }

    public static Texture2D GetNewTexture(int width, int height, string texName = "Picture")
    {
        var texture = new Texture2D(width, height, TextureFormat.RGB24, false)
        {
            name = texName
        };
        return texture;
    }

    public static Texture2D GetReadPixels(this Texture2D texture, bool isApply = false)
    {
        texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        if (isApply) texture.Apply();
        return texture;
    }

    public static void Destroy(this Sprite sprite)
    {
        if (sprite == null) return;
        sprite.texture.Destroy();
        Object.Destroy(sprite);
    }

    public static void Destroy(this Texture texture)
    {
        if (texture != null) Object.Destroy(texture);
    }
}
