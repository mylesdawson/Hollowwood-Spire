using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HitManager : MonoBehaviour
{
    float hitStunDuration = 0.1f;
    float hitStunTimer = 0f;
    float hitStopDuration = 0.1f;
    List<Collider2D> enemiesHit = new List<Collider2D>();
    Vignette vignette;
    float startingVignetteIntensity;
    float hitVignetteIntensity = 0.45f;
    float hitJumpXVelocity = 20f;
    float hitJumpYVelocity = 10f;
    float hitGravityUpMultiplier = 2f;

    void Awake()
    {
    }

    public void OnUpdate(ActionContext ctx)
    {
        if(ctx.CollisionsWithEnemies.Count > 0 && !ctx.IsHit)
        {
            TryStartHit(ctx);
        }

        if(ctx.IsHit)
        {
            UpdateHit(ctx, Time.deltaTime);
        }
    }

    void TryStartHit(ActionContext ctx)
    {
        ctx.IsHit = true;
        ctx.IsAttackLocked = true;
        hitStunTimer = hitStunDuration;

        if(ctx.Transform.TryGetComponent(out HitStop hitStop))
        {
            hitStop.Stop(hitStopDuration);
        }

        if(ctx.Transform.TryGetComponent(out FlashWhite flashWhite))
        {
            flashWhite.Flash();
        }

        if(ctx.Transform.TryGetComponent(out Volume volume))
        {
            if(volume.profile.TryGet(out vignette))
            {
                startingVignetteIntensity = vignette.intensity.value;
                vignette.intensity.value = hitVignetteIntensity;
            }
        }

        enemiesHit = ctx.CollisionsWithEnemies;
        // SoundEffectsManager.Instance.PlayEffect("PlayerDamage");
    }

    void UpdateHit(ActionContext ctx, float dt)
    {
        if(hitStunTimer > 0f)
        {
            bool shouldMoveForward = true;
            if(enemiesHit.Count > 0)
            {
                var enemy = enemiesHit[0];
                shouldMoveForward = ctx.Transform.position.x > enemy.transform.position.x;
            }

            ctx.VelocityX = shouldMoveForward ? hitJumpXVelocity : -hitJumpXVelocity;
            ctx.VelocityY = hitGravityUpMultiplier * hitJumpYVelocity;
        } else
        {
            ctx.IsAttackLocked = false;
            ctx.IsHit = false;
            if(ctx.Transform.TryGetComponent(out Volume volume))
            {
                if(volume.profile.TryGet(out vignette))
                {
                    vignette.intensity.value = startingVignetteIntensity;
                }
            }
            enemiesHit.Clear();   
        }

        hitStunTimer -= Time.deltaTime;

    }
}