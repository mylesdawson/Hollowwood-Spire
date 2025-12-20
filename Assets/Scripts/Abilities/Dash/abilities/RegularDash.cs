using System.Collections.Generic;
using UnityEngine;

public class RegularDash : DashBehavior
{
    public override string AbilityName => "Regular Dash";
    public override string AbilityDescription => "A straightforward dash in the facing direction, with an option to dash downwards.";
    public override AbilityType AbilityType => AbilityType.Dash;
    public override AbilitySubtype AbilitySubtype => AbilitySubtype.Override;
    readonly float downwardDashInputThreshold = -0.60f;
    float initialYInput = 0f;

    public RegularDash()
    {
        this.dashConfig = new DashConfig();
    }

    public override void OnStart(ActionContext ctx, List<AbilityStatMutation> mutations)
    {
        ctx.GravityConfig.SetGravityDownMultiplier(0);
        currentDashTimer = dashConfig.GetStat(AbilityStat.dashDuration, mutations);
        this.dashConfig.DecrementRemainingDashes();
        initialYInput = ctx.MovementInput.y;
        // SoundEffectsManager.Instance.PlayEffect("Dash");
    }

    public override bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> mutations)
    {
        var dashSpeed = this.dashConfig.GetStat(AbilityStat.dashSpeed, mutations);

        // Apply downward velocity if stick is pushed down
        if(initialYInput < downwardDashInputThreshold)
        {
            ctx.VelocityY = -dashSpeed;
        } else
        {
            float dashAmount = ctx.CurrentDirection > 0 ? dashSpeed : -dashSpeed;
            ctx.Velocity = new Vector2(dashAmount, 0);
        }

        currentDashTimer -= dt;

        // Return false to continue dashing, true when duration is complete
        bool shouldContinueDashing = currentDashTimer > 0;
        return shouldContinueDashing;
    }

    public override void OnEnd(ActionContext ctx, List<AbilityStatMutation> mutations)
    {
        ctx.GravityConfig.SetGravityDownMultiplier(1);
    }
}