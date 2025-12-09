public enum PlayerNames
{
    TheChosen
}


public class PlayerConfig
{
    public PlayerNames PlayerName { get; set; }
    public MoveConfig MoveConfig { get; set; }
    public JumpBehavior JumpAbility { get; set; }
    public GravityConfig GravityConfig { get; set; }
    public DashBehavior DashAbility { get; set; }
    public AttackBehavior AttackAbility { get; set; }

    public PlayerConfig(
        PlayerNames playerName,
        MoveConfig moveConfig,
        JumpBehavior jumpAbility,
        GravityConfig gravityConfig,
        DashBehavior dashAbility,
        AttackBehavior attackAbility)
    {
        PlayerName = playerName;
        MoveConfig = moveConfig;
        JumpAbility = jumpAbility;
        GravityConfig = gravityConfig;
        DashAbility = dashAbility;
        AttackAbility = attackAbility;
    }
}