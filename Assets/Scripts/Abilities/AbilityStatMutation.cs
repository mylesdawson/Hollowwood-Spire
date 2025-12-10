using System;
using System.Collections.Generic;
using System.Linq;

public enum AbilityStat
{
    // ATTACK ////
    attackDamage,
    attackCooldown,
    attackDuration,
    enemyKnockBackStrength,
    selfKnockBackStrength,
    pogoVelocityY,

    // DASH /////
    numDashes,
    dashDuration,
    dashSpeed,

    // MOVE //
    moveSpeed,

    // GRAVITY //
    gravityDownMultiplier,
    walledGravityDownMultiplier,

    // JUMP ////
    maxJumps,
    jumpHeight,

}


public enum StatOp
{
    Add,
    Multiply,
    Override
}

public class AbilityStatMutation
{
    public AbilityStat statType;
    public float value;
    public StatOp op;

    public AbilityStatMutation(AbilityStat statType, float value, StatOp op = StatOp.Add)
    {        
        this.statType = statType;
        this.value = value;
        this.op = op;
    }
}

public static class StatResolver
{
    /// <summary>
    /// Resolves a stat value with a base value and a list of mutations.
    /// Order: Add -> Multiply -> Override
    /// </summary>
    public static float ResolveStat(AbilityStat stat, float baseValue, List<AbilityStatMutation> mutations)
    {
        float result = baseValue;

        // Filter relevant mutations
        var relevant = mutations
            .Where(m => m.statType.Equals(stat))
            .ToList();

        // Apply Add operations
        foreach (var m in relevant.Where(m => m.op == StatOp.Add))
            result += m.value;

        // Apply Multiply operations
        foreach (var m in relevant.Where(m => m.op == StatOp.Multiply))
            result *= m.value;

        // Apply Override (last one wins)
        var overrideMutation = relevant.LastOrDefault(m => m.op == StatOp.Override);
        if (overrideMutation != null)
            result = overrideMutation.value;

        return result;
    }
}