

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RegularJump : JumpBehavior
{
    public override string AbilityName => "Regular Jump";
    public override string AbilityDescription => "A standard jump that can be held for higher jumps";
    public override AbilityType AbilityType => AbilityType.Jump;

    public RegularJump(List<AbilityStatMutation> statMutations): base(statMutations)
    {
    }

    public override void OnStart(ActionContext ctx, List<AbilityStatMutation> statMutations)
    {
        isJumping = true;
        if(ctx.CurrentState is Walled)
        {
            walledJumpTimer = walledJumpDuration;
            ctx.IsWallJumping = true;
            ctx.LeftWallAs = ctx.WalledStatus;
        } else
        {
            jumpHeldTimer = jumpHeldMaxTime;
        }
        numJumps--;
    }

    public override bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> statMutations)
    {
        if(walledJumpTimer > 0f)
        {
            ctx.Velocity = HandleWallJump(ctx.LeftWallAs);
            walledJumpTimer -= Time.deltaTime;
        } else
        {
            var res = new Vector2(0,0);
            if(this.canStartJump || ctx.IsCurrentlyJumping && !isJumpedLocked)
            {
                res = this.HandleRegularJump(ctx.IsCurrentlyJumping);
            }
            ctx.VelocityY = res.y > 0 ? res.y : ctx.VelocityY;
        }

        walledJumpTimer -= dt;
        jumpHeldTimer -= dt;

        if(walledJumpTimer > 0f) return true;
        return jumpHeldTimer > 0 && ctx.IsCurrentlyJumping;
    }

    public override void OnEnd(ActionContext ctx, List<AbilityStatMutation> statMutations)
    {
        ctx.IsWallJumping = false;
        isJumping = false;
    }


    private Vector2 HandleRegularJump(bool currentlyJumping)
    {
        float returnVelo = 0;
        if (currentlyJumping && jumpHeldTimer > 0f)
        {
            isJumping = true;
            var yVelo = Mathf.Sqrt(2f * config.GetGravityUpMultiplier() * config.GetStat(AbilityStat.jumpHeight, this.statMutations));
            returnVelo = yVelo;
        } else
        {
            isJumping = false;
        }

        if(jumpHeldTimer >= 0f) jumpHeldTimer -= Time.deltaTime;
        return new Vector2(0, returnVelo);
    }

    private Vector2 HandleWallJump(WalledStatus leftWallAs)
    {
        Vector2 returnVelo = Vector2.zero;
        returnVelo.x = leftWallAs == WalledStatus.Left ? config.GetWallJumpVelocityX() : -config.GetWallJumpVelocityX();
		returnVelo.y = Mathf.Sqrt(config.GetGravityUpMultiplier() * config.GetStat(AbilityStat.jumpHeight, this.statMutations));
        return returnVelo;
    }

}