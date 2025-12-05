

using System.Collections.Generic;
using UnityEngine;

public class RegularJump : JumpBehavior
{
    public RegularJump(List<JumpStatMutation> jumpStatMutations): base(jumpStatMutations)
    {
    }

    public override void OnStart(ActionContext ctx)
    {
        isJumping = true;
        if(ctx.CurrentState is Walled)
        {
            walledJumpTimer = walledJumpDuration;
            ctx.LeftWallAs = ctx.WalledStatus;
        } else
        {
            jumpHeldTimer = jumpHeldMaxTime;
        }
        numJumps--;
    }

    public override bool OnUpdate(ActionContext ctx, float dt)
    {
        if(walledJumpTimer > 0f)
        {
            ctx.Velocity = HandleWallJump(ctx.LeftWallAs);
            walledJumpTimer -= Time.deltaTime;
        } else if(ctx.CurrentState is Walled)
        {
            var wStatus = ctx.WalledStatus;
            walledJumpTimer = walledJumpDuration;
            ctx.LeftWallAs = wStatus;
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

        return jumpHeldTimer > 0 && ctx.IsCurrentlyJumping;
    }

    public override void OnEnd(ActionContext ctx)
    {
        isJumping = false;
    }


    private Vector2 HandleRegularJump(bool currentlyJumping)
    {
        float returnVelo = 0;
        if (currentlyJumping && jumpHeldTimer > 0f)
        {
            isJumping = true;
            var yVelo = Mathf.Sqrt(2f * config.GetGravityUpMultiplier() * config.GetJumpHeight());
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
		returnVelo.y = Mathf.Sqrt(config.GetGravityUpMultiplier() * config.GetJumpHeight());
        return returnVelo;
    }
}