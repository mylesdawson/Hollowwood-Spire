

public enum GravityStat
{
    gravityDownMultiplier,
    walledGravityDownMultiplier

}

public abstract class GravityStatMutation
{
    public GravityStat statType;

    protected GravityStatMutation(GravityStat statType)
    {
        this.statType = statType;
    }

    public abstract object GetData();
}


public class GravityDownMutation : GravityStatMutation
{
    public float data;

    public GravityDownMutation(float data) : base(GravityStat.gravityDownMultiplier) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}

public class WalledGravityDownMutation : GravityStatMutation
{
    public float data;

    public WalledGravityDownMutation(float data) : base(GravityStat.gravityDownMultiplier) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}
