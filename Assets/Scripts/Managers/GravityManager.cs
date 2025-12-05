using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GravityConfig
{
    float baseGravity;
    float baseGravityDownMultiplier;
    float baseWalledGravityDownMultiplier;

    public GravityConfig(float baseGravity = 80, float gravityDownMult = 1, float baseWalledGravityDownMultiplier = 1f)
    {
        this.baseGravity = baseGravity;
        this. baseGravityDownMultiplier = gravityDownMult;
        this.baseWalledGravityDownMultiplier = baseWalledGravityDownMultiplier;
    }

    public void SetGravityDownMultiplier(float newMultiplier)
    {
        baseGravityDownMultiplier = newMultiplier;
    }

    public float GetGravity()
    {
        return baseGravity;
    }

    public float GetGravityDownMultiplier()
    {
        return baseGravityDownMultiplier;
    }

    public float GetWalledGravityDownMultiplier(List<GravityStatMutation> mutations)
    {
        float result = baseWalledGravityDownMultiplier;
        var moveSpeedMutations = mutations.OfType<WalledGravityDownMutation>();
        foreach (var mutation in moveSpeedMutations)
        {
            result += mutation.data;
        }
        return result;
    }
}

public class GravityManager: MonoBehaviour
{
    public GravityConfig config;
    float slidingGraceTime = .15f;
    float slidingGraceTimer = 0f;
    float wallSlidingVelocity = 5f;
    bool inWalledState = false;

    List<GravityStatMutation> gravityStatMutations = new();

    void Awake()
    {
        config ??= new GravityConfig();
        // gravityStatMutations.Add(new WalledGravityDownMutation(-1f));
    }

    public void OnUpdate(ActionContext ctx, float dt)
    {
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
                ctx.VelocityY -= HandleGravity(config.GetWalledGravityDownMultiplier(gravityStatMutations), dt);
            }    
        } else
        {
            ctx.VelocityY = ctx.VelocityY - HandleGravity(dt);
        }

        slidingGraceTimer -= Time.deltaTime;
    }

    public float HandleGravity(float dt)
    {
        return config.GetGravity() * config.GetGravityDownMultiplier() * dt;
    }

    public float HandleGravity(float walledMultipler, float dt)
    {
        return config.GetGravity() * config.GetGravityDownMultiplier() * walledMultipler * dt;
    }

}