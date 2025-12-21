using System.Collections.Generic;
using UnityEngine;

public class JumpManager: MonoBehaviour
{
    public bool initialized = false;

    public JumpBehavior jump;
    public List<AbilityStatMutation> jumpStatMutations = new();

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

        if(ctx.IsCurrentlyJumping || ctx.IsWallJumping)
        {
            UpdateJump(ctx, Time.deltaTime);
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

    void UpdateJump(ActionContext ctx, float dt)
    {
        bool shouldContinueDashing = jump.OnUpdate(ctx, dt, jumpStatMutations);
        // this.dashAbilityMutations.ForEach(ability => ability.OnAbilityUpdate(ctx, dt));
        if(!shouldContinueDashing)
        {
            ctx.IsCurrentlyJumping = false;
            jump.OnEnd(ctx, jumpStatMutations);
            // this.dashAbilityMutations.ForEach(ability => ability.OnAbilityEnd(ctx));
        }
    }

    public List<Ability> GetAbilities()
    {
        List<Ability> abilities = new()
        {
            jump
        };
        return abilities;
    }

    public void AddAbility(Ability ability)
    {
        if(ability.AbilitySubtype == AbilitySubtype.Mutation)
        {
            Debug.LogWarning("not implemented!");
        } else
        {
            jump = (JumpBehavior)ability;
        }
    }
}