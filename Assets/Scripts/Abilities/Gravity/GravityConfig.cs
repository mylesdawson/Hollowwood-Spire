using System.Collections.Generic;

public class GravityConfig
{
    private readonly Dictionary<AbilityStat, float> modifiableStats;
    float baseGravity;
    public GravityConfig(float baseGravity = 20, float gravityDownMult = 1, float baseWalledGravityDownMultiplier = 1f)
    {
        this.baseGravity = baseGravity;


        modifiableStats = new()
        {
            { AbilityStat.gravityDownMultiplier, gravityDownMult },
            { AbilityStat.walledGravityDownMultiplier, baseWalledGravityDownMultiplier },
        };

    }

    public float GetStat(AbilityStat stat, List<AbilityStatMutation> mutations)
    {
        float result = modifiableStats[stat];
        return StatResolver.ResolveStat(stat, result, mutations);
    }

    public void SetGravityDownMultiplier(float newMultiplier)
    {
        modifiableStats[AbilityStat.gravityDownMultiplier] = newMultiplier;
    }

    public float GetGravity()
    {
        return baseGravity;
    }
}