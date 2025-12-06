using UnityEngine;

public class ParentColorChanger : MonoBehaviour
{
    // Define the color you want children to change to
    public Color childColor = Color.black;

#if UNITY_EDITOR
    // This function is called when a value is changed in the Inspector (including hierarchy changes)
    void OnValidate()
    {
        // Change color for all SpriteRenderer components in children (and the object itself)
        foreach (SpriteRenderer spriteRenderer in GetComponentsInChildren<SpriteRenderer>())
        {
            // Ensure you don't change the parent's color if it also has a SpriteRenderer
            if (spriteRenderer.gameObject != this.gameObject)
            {
                spriteRenderer.color = childColor;
            }
        }
    }
#endif
}