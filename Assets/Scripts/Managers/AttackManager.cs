using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(HitStop))]
public class AttackManager : MonoBehaviour
{
    public bool initialized = false;
    // Stop allowing attacks
    public bool isAttackLocked = false;
    [SerializeField] public Transform attackPrefab;
    public AttackBehavior attack;
    public List<AbilityStatMutation> attackStatMutations = new();
    public List<Ability> attackAbilityMutations = new();

    public void Initialize(AttackBehavior attackAbility)
    {
        attack = attackAbility;
        initialized = true;
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
            !ctx.IsAttacking &&
            ctx.CurrentState is not Walled
            )
        {
            attack.cooldownTimer = attack.config.GetStat(AbilityStat.attackCooldown, attackStatMutations);
            attack.OnStart(ctx, attackStatMutations);
            this.attackAbilityMutations.ForEach(ability => ability.OnStart(ctx, attackStatMutations));
        }
    }

    void UpdateAttack(ActionContext ctx, float dt)
    {
        var shouldContinue = attack.OnUpdate(ctx, dt, attackStatMutations);
        this.attackAbilityMutations.ForEach(ability => ability.OnUpdate(ctx, dt, attackStatMutations));
        if(!shouldContinue)
        {
            attack.OnEnd(ctx, attackStatMutations);
            this.attackAbilityMutations.ForEach(ability => ability.OnEnd(ctx, attackStatMutations));
        }
    }
}