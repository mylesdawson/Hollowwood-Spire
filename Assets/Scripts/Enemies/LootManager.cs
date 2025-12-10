

using System.Collections.Generic;
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
    public List<Ability> abilities;
    public List<AbilityMutation> abilityMutations;
    public List<AbilityStatMutation> abilityStatMutations;

    void Awake()
    {
        var attackPrefab = (GameObject)Resources.Load("Attack");
        abilities = new()
        {
            new RegularDash(),
            new OmniDash(),
            new RegularAttack(attackPrefab.transform),
            new HeavyAttack(attackPrefab.transform),
        };


        abilityMutations = new()
        {
            new PlantSpawnerMove(),
            new ResetDashMutation(),
            new InvincibleDashMutation(),
            new KnockupAttackMutation()
        };
    }


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