
using System.Collections.Generic;

public class JumpConfig
{
    private readonly Dictionary<AbilityStat, float> modifiableStats;
    float baseGravityUpMultiplier;
    float baseWallJumpVelocityX;
    float baseHitJumpVelocityX;

    public JumpConfig(int baseMaxJumps = 1, float baseJumpHeight = 100f, float baseGravityUpMultiplier = 2f, float baseWallJumpVelocityX = 10f, float baseHitJumpVelocityX = 12f)
    {
        this.baseGravityUpMultiplier = baseGravityUpMultiplier;
        this.baseWallJumpVelocityX = baseWallJumpVelocityX;
        this.baseHitJumpVelocityX = baseHitJumpVelocityX;

        modifiableStats = new()
        {
            { AbilityStat.maxJumps, baseMaxJumps },
            { AbilityStat.jumpHeight, baseJumpHeight },
        };

    }

    public float GetStat(AbilityStat stat, List<AbilityStatMutation> mutations)
    {
        float result = modifiableStats[stat];
        return StatResolver.ResolveStat(stat, result, mutations);
    }

    public float GetGravityUpMultiplier()
    {
        return baseGravityUpMultiplier;
    }

    public float GetWallJumpVelocityX()
    {
        return baseWallJumpVelocityX;
    }

    public float GetHitJumpVelocityX()
    {
        return baseHitJumpVelocityX;
    }
}