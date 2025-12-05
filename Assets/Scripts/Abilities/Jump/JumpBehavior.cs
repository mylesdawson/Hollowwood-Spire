using System.Collections.Generic;

public abstract class JumpBehavior
{
    public int numJumps;
    public float walledJumpTimer = 0f;
    public float walledJumpDuration = .2f;
    public float jumpHeldMaxTime = .1f;
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
    protected List<JumpStatMutation> jumpStatMutations;

    protected JumpBehavior(List<JumpStatMutation> jumpStatMutations)
    {
        config = new JumpConfig();
        numJumps = config.GetMaxJumps();
        this.jumpStatMutations = jumpStatMutations;
    }

    /// <summary>
    /// Called when the dash state begins
    /// </summary>
    public abstract void OnStart(ActionContext ctx);

    /// <summary>
    /// Called every frame during the dash. Return true if dash should end.
    /// </summary>
    public abstract bool OnUpdate(ActionContext ctx, float dt);

    /// <summary>
    /// Called when the dash state ends
    /// </summary>
    public abstract void OnEnd(ActionContext ctx);
}