

public enum MoveStatType
{
    moveSpeed,
}

public abstract class MoveStatMutation
{
    public MoveStatType statType;

    protected MoveStatMutation(MoveStatType statType)
    {
        this.statType = statType;
    }

    public abstract object GetData();
}


public class MoveSpeedMutation : MoveStatMutation
{
    public float data;

    public MoveSpeedMutation(float data) : base(MoveStatType.moveSpeed) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}
