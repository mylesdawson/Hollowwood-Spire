

using System.Collections.Generic;
using System.Linq;

public class DashConfig
{
    int baseMaxDashes;
    int remainingDashes;
    float baseDashDuration;
    float baseDashSpeed;

    public DashConfig(int maxDashes = 1, float dashDuration = .2f, float dashSpeed = 15f)
    {
        baseMaxDashes = maxDashes;
        baseDashDuration = dashDuration;
        baseDashSpeed = dashSpeed;

        remainingDashes = baseMaxDashes;
    }

    public float GetDashSpeed(List<DashStatMutation> mutations)
    {
        float result = baseDashSpeed;
        var numDashesMutations = mutations.OfType<DashSpeedMutation>();
        foreach (var mutation in numDashesMutations)
        {
            result += mutation.data;
        }
        return result;
    }

    public float GetDashDuration(List<DashStatMutation> mutations)
    {
        float result = baseDashDuration;
        var numDashesMutations = mutations.OfType<DashDurationMutation>();
        foreach (var mutation in numDashesMutations)
        {
            result += mutation.data;
        }
        return result;
    }

    public int GetMaxDashes(List<DashStatMutation> mutations)
    {
        int result = baseMaxDashes;
        var numDashesMutations = mutations.OfType<NumDashesMutation>();
        foreach (var mutation in numDashesMutations)
        {
            result += mutation.data;
        }
        return result;
    }


    public int GetRemainingDashes()
    {
        return remainingDashes;
    }

    public void ResetRemainingDashes(List<DashStatMutation> mutations)
    {
        remainingDashes = this.GetMaxDashes(mutations);
    }

    public int IncrementRemainingDashes()
    {
        if(remainingDashes < baseMaxDashes)
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