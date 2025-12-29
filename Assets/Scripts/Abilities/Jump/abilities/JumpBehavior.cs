using System.Collections.Generic;

public abstract class JumpBehavior: Ability
{
    public int numJumps;
    public float walledJumpTimer = 0f;
    public float walledJumpDuration = .3f;
    public float jumpHeldMaxTime = .3f;
    public float jumpHeldTimer = 0f;
    public bool isJumpedLocked = false;
    // Can the player currently jump
    public bool canStartJump {
        get => !isJumpedLocked && numJumps > 0;
    }
    public bool wasJumping = false;
    private bool _isJumping = false;
    public bool isJumping {
        get => _isJumping;
        set {
            wasJumping = _isJumping;
            _isJumping = value;
        }
    }
    public JumpConfig config;
    protected List<AbilityStatMutation> statMutations;

    protected JumpBehavior(List<AbilityStatMutation> statMutations)
    {
        config = new JumpConfig();
        numJumps = (int)config.GetStat(AbilityStat.maxJumps, statMutations);
        this.statMutations = statMutations;
    }
}