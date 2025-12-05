using UnityEngine;
using UnityEngine.Events;

public class Attackable : MonoBehaviour
{
    [ReadOnly] public UnityEvent<int, Vector2, float, float> onAttacked = new();

    public void TakeDamage(int damage, Vector2 direction, float knockbackStrength, float hitStop = 0f)
    {
        onAttacked?.Invoke(damage, direction, knockbackStrength, hitStop);
    }
}