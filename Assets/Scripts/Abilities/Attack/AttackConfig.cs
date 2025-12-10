using System.Collections.Generic;
using UnityEngine;

public class AttackConfig
{
    private readonly Dictionary<AbilityStat, float> modifiableStats;
    // For joysticks
    public readonly float downwardAttackInputThreshold = -0.30f;
    public readonly float sidewaysAttackOverrideThreshold = .6f;
    public readonly float upwardAttackInputThreshold = 0.40f;
    public readonly float selfKnockbackStrengthDuration = 0.1f;
    public readonly float pogoTimerDuration = 0.1f;

    public readonly LayerMask enemyLayer;
    public Transform attackPrefab;
    public readonly float baseAttackDistance = 5f;
    public BoxCollider2D attackCollider;

    public AttackConfig(
        Transform attackPrefab,
        float baseAttackDamage = 2, 
        float baseAttackCooldown = .6f,
        float baseAttackDuration = .2f,
        float baseEnemyKnockBackStrength = 10f, 
        float baseSelfKnockBackStrength = 5f, 
        float basePogoVelocityY = 10f,
        float baseAttackDistance = 1.5f
        )
    {
        modifiableStats = new()
        {
            { AbilityStat.attackDamage, baseAttackDamage },
            { AbilityStat.attackCooldown, baseAttackCooldown },
            { AbilityStat.attackDuration, baseAttackDuration },
            { AbilityStat.enemyKnockBackStrength, baseEnemyKnockBackStrength },
            { AbilityStat.selfKnockBackStrength, baseSelfKnockBackStrength },
            { AbilityStat.pogoVelocityY, basePogoVelocityY },
        };
        this.attackPrefab = attackPrefab;
        this.baseAttackDistance = baseAttackDistance;
        this.enemyLayer = LayerMask.GetMask("Enemy");
        this.attackCollider = attackPrefab.GetComponent<BoxCollider2D>();
    }

    public float GetStat(AbilityStat stat, List<AbilityStatMutation> mutations)
    {
        float result = modifiableStats[stat];
        return StatResolver.ResolveStat(stat, result, mutations);
    }
}
