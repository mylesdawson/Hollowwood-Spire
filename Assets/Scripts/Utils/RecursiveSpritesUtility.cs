using UnityEngine;

public static class RecursiveSpritesUtility
{
    public static void RecursiveSetColor(Transform t, Color color)
    {
        if (t.GetComponent<SpriteRenderer>() != null)
        {
            var art = t.GetComponent<SpriteRenderer>();
            art.color = color;
        }
        for (var i = 0; i < t.childCount; i++)
        {
            RecursiveSetColor(t.GetChild(i), color);
        }
    }
}
