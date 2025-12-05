

public enum DashStatType
{
    numDashes,
    dashDuration,
    dashSpeed
}

public abstract class DashStatMutation
{
    public DashStatType statType;

    protected DashStatMutation(DashStatType statType)
    {
        this.statType = statType;
    }

    public abstract object GetData();
}

public class NumDashesMutation : DashStatMutation
{
    public int data;

    public NumDashesMutation(int numDashes) : base(DashStatType.numDashes) 
    { 
        data = numDashes;
    }

    public override object GetData() => data;
}

public class DashSpeedMutation : DashStatMutation
{
    public float data;

    public DashSpeedMutation(float dashSpeed) : base(DashStatType.dashSpeed) 
    { 
        data = dashSpeed;
    }

    public override object GetData() => data;
}

public class DashDurationMutation : DashStatMutation
{
    public float data;

    public DashDurationMutation(float dashDuration) : base(DashStatType.dashDuration) 
    { 
        data = dashDuration;
    }

    public override object GetData() => data;
}