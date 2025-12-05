using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class Healthable : MonoBehaviour
{
    public int health = 1;
    [ReadOnly] public UnityEvent onDie = new();

    public void LoseHealth(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            onDie?.Invoke();
        }
    }
}