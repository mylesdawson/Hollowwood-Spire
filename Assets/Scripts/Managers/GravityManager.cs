using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravityManager: MonoBehaviour
{
    public bool initialized = false;
    public GravityConfig config;
    float slidingGraceTime = .15f;
    float slidingGraceTimer = 0f;
    float wallSlidingVelocity = 5f;
    bool inWalledState = false;

    List<AbilityStatMutation> gravityStatMutations = new();

    public void Initialize(GravityConfig config)
    {
        this.config = config;
        initialized = true;
    }

    public void OnUpdate(ActionContext ctx, float dt)
    {
        if(!initialized) return;

        if(ctx.CurrentState is Walled && ctx.PreviousState is not Walled && !inWalledState)
        {
            inWalledState = true;
            slidingGraceTimer = slidingGraceTime;
        } else if(ctx.CurrentState is not Walled)
        {
            inWalledState = false;
        }

        if(ctx.CurrentState is Walled)
        {
            if (slidingGraceTimer > 0 && ctx.VelocityY <= 0)
            {
                ctx.VelocityY = 0;
            } else
            {
                ctx.VelocityY = Mathf.Clamp(ctx.VelocityY, -wallSlidingVelocity, float.MaxValue);
                ctx.VelocityY -= HandleGravity(config.GetStat(AbilityStat.walledGravityDownMultiplier, gravityStatMutations), dt);
            }    
        } else
        {
            ctx.VelocityY = ctx.VelocityY - HandleGravity(dt);
        }

        slidingGraceTimer -= Time.deltaTime;
    }

    public float HandleGravity(float dt)
    {
        return config.GetGravity() * config.GetStat(AbilityStat.gravityDownMultiplier, gravityStatMutations) * dt;
    }

    public float HandleGravity(float walledMultipler, float dt)
    {
        return config.GetGravity() * config.GetStat(AbilityStat.gravityDownMultiplier, gravityStatMutations) * walledMultipler * dt;
    }

}