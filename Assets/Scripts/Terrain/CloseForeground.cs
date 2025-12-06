
using UnityEngine;

public class CloseForeground :MonoBehaviour
{
    void Start()
    {
        RecursiveSpritesUtility.RecursiveSetColor(transform, Color.black);
    }
}