using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackConfig
{
    int baseAttackDamage;
    float baseAttackCooldown;
    float baseAttackDuration;
    float baseEnemyKnockBackStrength;
    float baseSelfKnockBackStrength;
    float basePogoVelocityY;
    // For joysticks
    public readonly float downwardAttackInputThreshold = -0.30f;
    public readonly float upwardAttackInputThreshold = 0.40f;
    public readonly float selfKnockbackStrengthDuration = 0.1f;
    public readonly float pogoTimerDuration = 0.1f;

    public readonly LayerMask enemyLayer;
    public Transform attackPrefab;
    public BoxCollider2D attackCollider;

    public AttackConfig(
        Transform attackPrefab,
        int baseAttackDamage = 2, 
        float baseAttackCooldown = .6f, 
        float baseAttackDuration = 0.2f,
        float baseEnemyKnockBackStrength = 10f, 
        float baseSelfKnockBackStrength = 5f, 
        float basePogoVelocityY = 10f
        )
    {
        this.baseAttackDamage = baseAttackDamage;
        this.baseAttackCooldown = baseAttackCooldown;
        this.baseAttackDuration = baseAttackDuration;
        this.baseEnemyKnockBackStrength = baseEnemyKnockBackStrength;
        this.baseSelfKnockBackStrength = baseSelfKnockBackStrength;
        this.basePogoVelocityY = basePogoVelocityY;
        this.attackPrefab = attackPrefab;
        this.enemyLayer = LayerMask.GetMask("Enemy");
        this.attackCollider = attackPrefab.GetComponent<BoxCollider2D>();
    }

    public int GetAttackDamage(List<AttackStatMutation> mutations)
    {
        var result = baseAttackDamage;
        var attackDamageMutation = mutations.OfType<AttackDamageMutation>();
        foreach (var mutation in attackDamageMutation)
        {
            result += mutation.data;
        }
        return result;
    }

    public float GetAttackCooldown(List<AttackStatMutation> mutations)
    {
        var result = baseAttackCooldown;
        var attackDamageMutation = mutations.OfType<AttackCooldownMutation>();
        foreach (var mutation in attackDamageMutation)
        {
            result += mutation.data;
        }
        return result;
    }

    public float GetAttackDuration(List<AttackStatMutation> mutations)
    {
        var result = baseAttackDuration;
        var attackDamageMutation = mutations.OfType<AttackDurationMutation>();
        foreach (var mutation in attackDamageMutation)
        {
            result += mutation.data;
        }
        return result;
    }

    public float GetEnemyKnockBackStrength(List<AttackStatMutation> mutations)
    {
        var result = baseEnemyKnockBackStrength;
        var attackDamageMutation = mutations.OfType<EnemyKnockBackStrengthMutation>();
        foreach (var mutation in attackDamageMutation)
        {
            result += mutation.data;
        }
        return result;
    }

    public float GetSelfKnockBackStrength(List<AttackStatMutation> mutations)
    {
        var result = baseSelfKnockBackStrength;
        var attackDamageMutation = mutations.OfType<SelfKnockBackStrengthMutation>();
        foreach (var mutation in attackDamageMutation)
        {
            result += mutation.data;
        }
        return result;
    }

    public float GetPogoVelocityY(List<AttackStatMutation> mutations)
    {
        var result = basePogoVelocityY;
        var attackDamageMutation = mutations.OfType<PogoVelocityYMutation>();
        foreach (var mutation in attackDamageMutation)
        {
            result += mutation.data;
        }
        return result;
    }
}


[RequireComponent(typeof(HitStop))]
public class AttackManager : MonoBehaviour
{
    // Stop allowing attacks
    public bool isAttackLocked = false;
    [SerializeField] public Transform attackPrefab;
    public AttackBehavior attack;
    List<AttackStatMutation> attackStatMutations = new();
    public List<AbilityMutation> attackAbilityMutations = new();

    void Awake()
    {
        attack = new RegularAttack(attackPrefab, attackStatMutations);
        attackStatMutations.Add(new AttackDamageMutation(2));
        attackStatMutations.Add(new AttackCooldownMutation(.2f));
        attackStatMutations.Add(new EnemyKnockBackStrengthMutation(5f));
        attackStatMutations.Add(new PogoVelocityYMutation(5f));
        // attackAbilityMutations.Add(new KnockupAttackMutation());
    }

    public void OnUpdate(ActionContext ctx)
    {
        if(ctx.DidAttackThisFrame)
        {
            TryStartAttack(ctx);
        }

        if(ctx.IsAttacking)
        {
            UpdateAttack(ctx, Time.deltaTime);
        }
        // cooldownTimer is unique as it needs to count down even when attack is not happening
        attack.cooldownTimer -= Time.deltaTime;
    }

    void TryStartAttack(ActionContext ctx)
    {
        if( attack.attackTimer <= 0f && 
            ctx.DidAttackThisFrame && 
            attack.cooldownTimer <= 0f && 
            !ctx.IsAttackLocked && 
            !ctx.IsAttacking
            )
        {
            attack.cooldownTimer = attack.config.GetAttackCooldown(attackStatMutations);
            attack.OnStart(ctx);
            this.attackAbilityMutations.ForEach(ability => ability.OnAbilityStart(ctx));
        }
    }

    void UpdateAttack(ActionContext ctx, float dt)
    {
        var shouldContinue = attack.OnUpdate(ctx, dt);
        this.attackAbilityMutations.ForEach(ability => ability.OnAbilityUpdate(ctx, dt));
        if(!shouldContinue)
        {
            attack.OnEnd(ctx);
            this.attackAbilityMutations.ForEach(ability => ability.OnAbilityEnd(ctx));
        }
    }
}