using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Healthable : MonoBehaviour
{
    public float health = 1;
    [ReadOnly] public UnityEvent onDie = new();

    public void LoseHealth(float amount)
    {
        health -= amount;
        if(health <= 0)
        {
            onDie?.Invoke();
        }
    }
}