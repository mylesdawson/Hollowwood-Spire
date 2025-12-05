

using UnityEngine;
using UnityEngine.VFX;

public class SelfDestructVFX : MonoBehaviour
{
    [SerializeField] string maxLifetimeName = "MaxLifetime";
    VisualEffect vfx;

    void Awake()
    {
        vfx = GetComponent<VisualEffect>();
        Destroy(gameObject, vfx.GetFloat(maxLifetimeName));
    }
}