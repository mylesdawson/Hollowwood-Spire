using System.Collections.Generic;
using UnityEngine;

public class JumpConfig
{
    int baseMaxJumps;
    float baseJumpHeight;
    float baseGravityUpMultiplier;
    float baseWallJumpVelocityX;
    float baseHitJumpVelocityX;

    public JumpConfig(int baseMaxJumps = 1, float baseJumpHeight = 10f, float baseGravityUpMultiplier = 2f, float baseWallJumpVelocityX = 10f, float baseHitJumpVelocityX = 12f)
    {
        this.baseMaxJumps = baseMaxJumps;
        this.baseJumpHeight = baseJumpHeight;
        this.baseGravityUpMultiplier = baseGravityUpMultiplier;
        this.baseWallJumpVelocityX = baseWallJumpVelocityX;
        this.baseHitJumpVelocityX = baseHitJumpVelocityX;
    }

    public int GetMaxJumps()
    {
        return baseMaxJumps;
    }

    public float GetJumpHeight()
    {
        return baseJumpHeight;
    }

    public float GetGravityUpMultiplier()
    {
        return baseGravityUpMultiplier;
    }

    public float GetWallJumpVelocityX()
    {
        return baseWallJumpVelocityX;
    }

    public float GetHitJumpVelocityX()
    {
        return baseHitJumpVelocityX;
    }
}



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
    public List<JumpStatMutation> jumpStatMutations = new();

    void Awake()
    {
        // jump = new RegularJump(jumpStatMutations);
        // jumpStatMutations.Add(new MaxJumpsMutation(1));
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
            jump.OnStart(ctx);
            // this.dashAbilityMutations.ForEach(ability => ability.OnAbilityStart(ctx));
        }
    }

    void UpdateDash(ActionContext ctx, float dt)
    {
        bool shouldContinueDashing = jump.OnUpdate(ctx, dt);
        // this.dashAbilityMutations.ForEach(ability => ability.OnAbilityUpdate(ctx, dt));
        if(!shouldContinueDashing)
        {
            ctx.IsDashing = false;
            jump.OnEnd(ctx);
            // this.dashAbilityMutations.ForEach(ability => ability.OnAbilityEnd(ctx));
        }
    }
}