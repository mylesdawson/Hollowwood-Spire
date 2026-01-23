

using System;
using UnityEngine;
using UnityEngine.Events;

public class EventBus : MonoBehaviour
{
    public static EventBus Instance; 
    public Action<GameObject> onEnemyDeath;
    public Action<GameObject> onLootTouched;
    public Action<Ability> onAbilityLooted;
    public Action onStartGameClicked;
    // float: health lost, float: remaining health``
    public UnityEvent<float, float> onPlayerLostHealth;
    // percentage of remaining health
    public UnityEvent <float> onEnemyLostHealth;

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