using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehavior
{
    public float cooldownTimer = 0f;
    public float attackTimer = 0f;
    public float pogoTimer = 0f;
    public float selfKnockbackStrengthTimer = 0f;
    public AttackConfig config;
    protected List<AttackStatMutation> attackMutations;
    public HashSet<int> attackablesHitThisAttack = new HashSet<int>();

    protected AttackBehavior(List<AttackStatMutation> attackMutations)
    {
        this.attackMutations = attackMutations;
    }

    /// <summary>
    /// Called when the dash state begins
    /// </summary>
    public abstract void OnStart(ActionContext ctx);

    /// <summary>
    /// Called every frame during the dash. Return true if dash should end.
    /// </summary>
    public abstract bool OnUpdate(ActionContext ctx, float dt);

    /// <summary>
    /// Called when the dash state ends
    /// </summary>
    public abstract void OnEnd(ActionContext ctx);
}