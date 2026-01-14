using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Triggers a camera shake effect using Cinemachine impulse source.
    /// </summary>
    /// <param name="intensity">The intensity of the shake (default: 1.0f)</param>
    /// <param name="direction">The direction of the impulse (default: Vector3.up)</param>
    public void ShakeCamera(CinemachineImpulseSource source, float intensity = 1.0f, Vector3? direction = null)
    {
        if (source == null)
        {
            Debug.LogWarning("CinemachineImpulseSource not assigned or found on this GameObject!");
            return;
        }

        Vector3 impulseDirection = direction ?? Vector3.up;
        source.GenerateImpulse(impulseDirection * intensity);
    }
}