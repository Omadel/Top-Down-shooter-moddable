using System.IO;
using UnityEngine;

public static class Loader
{

    public static Sprite LoadSprite(string path, int? pixelsPerUnit = null)
    {
        if (!File.Exists(path))
        {
            Debug.LogError("No SpriteFound !");
            return null;
        }
        Texture2D spriteTexture = new Texture2D(1, 1);
        spriteTexture.LoadImage(File.ReadAllBytes(path));
        spriteTexture.filterMode = FilterMode.Point;
        if (pixelsPerUnit == null) pixelsPerUnit = Mathf.Max(spriteTexture.width, spriteTexture.height);
        Sprite sprite = Sprite.Create(spriteTexture, new Rect(0, 0, spriteTexture.width, spriteTexture.height), new Vector2(.5f, .5f), pixelsPerUnit.Value);
        sprite.name = Path.GetFileNameWithoutExtension(path);
        return sprite;
    }
}
