

using System;
using UnityEngine;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance; 
    public Action<LootRarity> onEnemyDeath;

    void Awake()
    {
        // if (Instance != null && Instance != this)
        // {
        //     Destroy(this.gameObject);
        //     return;
        // }

        Instance = this;
        // DontDestroyOnLoad(this.gameObject);
    }
}