using System.Collections.Generic;
using UnityEngine;

public class JumpManager: MonoBehaviour
{
    public bool initialized = false;
    // Lock player from jumping
    public bool isJumpedLocked = false;
    // Can the player currently jump
    public bool wasJumping = false;
    private bool _isJumping = false;
    public bool isJumping {
        get => _isJumping;
        set {
            wasJumping = _isJumping;
            _isJumping = value;
        }
    }

    public JumpBehavior jump;
    public List<AbilityStatMutation> jumpStatMutations = new();

    void Awake()
    {

    }

    public void Initialize(JumpBehavior jump)
    {
        this.jump = jump;
        initialized = true;
    }

    public void OnUpdate(ActionContext ctx)
    {
        if(!initialized) return;
        if(ctx.DidJumpThisFrame)
        {
            TryStartJump(ctx);
        }

        if(ctx.IsCurrentlyJumping)
        {
            UpdateDash(ctx, Time.deltaTime);
        }
    }

    void TryStartJump(ActionContext ctx)
    {
        if(ctx.DidJumpThisFrame && jump.canStartJump )
        {
            jump.OnStart(ctx, jumpStatMutations);
            // this.dashAbilityMutations.ForEach(ability => ability.OnAbilityStart(ctx));
        }
    }

    void UpdateDash(ActionContext ctx, float dt)
    {
        bool shouldContinueDashing = jump.OnUpdate(ctx, dt, jumpStatMutations);
        // this.dashAbilityMutations.ForEach(ability => ability.OnAbilityUpdate(ctx, dt));
        if(!shouldContinueDashing)
        {
            ctx.IsDashing = false;
            jump.OnEnd(ctx, jumpStatMutations);
            // this.dashAbilityMutations.ForEach(ability => ability.OnAbilityEnd(ctx));
        }
    }
}