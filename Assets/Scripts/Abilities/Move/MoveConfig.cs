

using System.Collections.Generic;

public class MoveConfig
{
    private readonly Dictionary<AbilityStat, float> modifiableStats;
    float moveThreshold;

    public MoveConfig(float baseMoveSpeed = 15f, float moveThreshold = .3f)
    {   
        modifiableStats = new()
        {
            { AbilityStat.moveSpeed, baseMoveSpeed },
        };

        this.moveThreshold = moveThreshold;
    }

    public float GetStat(AbilityStat stat, List<AbilityStatMutation> mutations)
    {
        float result = modifiableStats[stat];
        return StatResolver.ResolveStat(stat, result, mutations);
    }

    public float GetMoveThreshold()
    {
        return moveThreshold;
    }
}   