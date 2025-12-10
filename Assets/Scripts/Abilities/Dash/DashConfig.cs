

using System.Collections.Generic;

public class DashConfig
{
    private readonly Dictionary<AbilityStat, float> modifiableStats;

    int remainingDashes;

    public DashConfig(int baseMaxDashes = 1, float baseDashDuration = .2f, float baseDashSpeed = 35f)
    {
        modifiableStats = new()
        {
            { AbilityStat.numDashes, baseMaxDashes },
            { AbilityStat.dashDuration, baseDashDuration },
            { AbilityStat.dashSpeed, baseDashSpeed },
        };

        remainingDashes = baseMaxDashes;
    }

    public float GetStat(AbilityStat stat, List<AbilityStatMutation> mutations)
    {
        float result = modifiableStats[stat];
        return StatResolver.ResolveStat(stat, result, mutations);
    }

    public int GetRemainingDashes()
    {
        return remainingDashes;
    }

    public void ResetRemainingDashes(List<AbilityStatMutation> mutations)
    {
        remainingDashes = (int)GetStat(AbilityStat.numDashes, mutations);
    }

    public int IncrementRemainingDashes(List<AbilityStatMutation> mutations)
    {
        if(remainingDashes < (int)GetStat(AbilityStat.numDashes, mutations))
        {
            remainingDashes++;
        }
        return remainingDashes;
    }

    public int DecrementRemainingDashes()
    {
        if(remainingDashes > 0) remainingDashes--;
        return remainingDashes;
    }
}