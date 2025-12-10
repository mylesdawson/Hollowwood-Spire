using UnityEngine;
using UnityEngine.Events;

public class Attackable : MonoBehaviour
{
    [ReadOnly] public UnityEvent<float, Vector2, float, float> onAttacked = new();

    public void TakeDamage(float damage, Vector2 direction, float knockbackStrength, float hitStop = 0f)
    {
        onAttacked?.Invoke(damage, direction, knockbackStrength, hitStop);
    }
}