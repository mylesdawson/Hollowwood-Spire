

using UnityEngine;

public enum LootRarity
{
    none,
    standard,
    rare,
    legendary
}

public class LootManager : MonoBehaviour
{
    
    void Start()
    {
        EventBus.Instance.onEnemyDeath += OnEnemyDeath;
    }

    void OnDisable()
    {
        EventBus.Instance.onEnemyDeath -= OnEnemyDeath;
    }

    void OnEnemyDeath(LootRarity rarity)
    {
        Debug.Log("enemy died!!!");
    }
}