public enum JumpStatType
{
    maxJumps,
    jumpHeight,
}

public abstract class JumpStatMutation
{
    public JumpStatType statType;

    protected JumpStatMutation(JumpStatType statType)
    {
        this.statType = statType;
    }

    public abstract object GetData();
}


public class MaxJumpsMutation : JumpStatMutation
{
    public int data;

    public MaxJumpsMutation(int data) : base(JumpStatType.maxJumps) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}

public class JumpHeightMutation : JumpStatMutation
{
    public float data;

    public JumpHeightMutation(float data) : base(JumpStatType.maxJumps) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}

