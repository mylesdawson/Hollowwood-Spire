

public enum AttackStatType
{
    attackDamage,
    attackCooldown,
    enemyKnockBackStrength,
    selfKnockBackStrength,
    pogoVelocityY
}

public abstract class AttackStatMutation
{
    public AttackStatType statType;

    protected AttackStatMutation(AttackStatType statType)
    {
        this.statType = statType;
    }

    public abstract object GetData();
}

public class AttackDamageMutation : AttackStatMutation
{
    public int data;

    public AttackDamageMutation(int data) : base(AttackStatType.attackDamage) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}

public class AttackCooldownMutation : AttackStatMutation
{
    public float data;

    public AttackCooldownMutation(float data) : base(AttackStatType.attackCooldown) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}

public class AttackDurationMutation : AttackStatMutation
{
    public float data;

    public AttackDurationMutation(float data) : base(AttackStatType.attackCooldown) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}


public class EnemyKnockBackStrengthMutation : AttackStatMutation
{
    public float data;

    public EnemyKnockBackStrengthMutation(float data) : base(AttackStatType.enemyKnockBackStrength) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}

public class SelfKnockBackStrengthMutation : AttackStatMutation
{
    public float data;

    public SelfKnockBackStrengthMutation(float data) : base(AttackStatType.selfKnockBackStrength) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}

public class PogoVelocityYMutation : AttackStatMutation
{
    public float data;

    public PogoVelocityYMutation(float data) : base(AttackStatType.pogoVelocityY) 
    { 
        this.data = data;
    }

    public override object GetData() => data;
}