

using System.Collections.Generic;
using UnityEngine;

public class DashManager: MonoBehaviour
{
    bool initialized = false;
    public DashBehavior dashAbility;
    public List<DashStatMutation> dashStatMutations = new();
    public List<AbilityMutation> dashAbilityMutations = new();

    void Awake()
    {
        // dashStatMutations.Add(new DashSpeedMutation(2f));
        // dashStatMutations.Add(new NumDashesMutation(1));
        // dashAbilityMutations.Add(new ResetDashMutation());
        // dashAbilityMutations.Add(new InvincibleDashMutation());
    }

    public void Initialize(DashBehavior dashAbility)
    {
        this.dashAbility = dashAbility;
        initialized = true;
    }

    public void OnUpdate(ActionContext ctx)
    {
        if(!initialized) return;
        if(dashAbility == null) return;

        if(ctx.DidDashThisFrame)
        {
            TryStartDash(ctx);
        }

        if(ctx.IsDashing)
        {
            UpdateDash(ctx, Time.deltaTime);
        }
    }

    void TryStartDash(ActionContext ctx)
    {
        if(dashAbility.dashConfig.GetRemainingDashes() == 0 || ctx.IsDashing || ctx.IsDashLocked)
            return;

        ctx.IsDashing = true;
        dashAbility.OnDashStart(ctx);
        this.dashAbilityMutations.ForEach(ability => ability.OnAbilityStart(ctx));
    }

    void UpdateDash(ActionContext ctx, float dt)
    {
        bool shouldContinueDashing = dashAbility.UpdateDash(ctx, dt);
        this.dashAbilityMutations.ForEach(ability => ability.OnAbilityUpdate(ctx, dt));
        if(!shouldContinueDashing)
        {
            ctx.IsDashing = false;
            dashAbility?.OnDashEnd(ctx);
            this.dashAbilityMutations.ForEach(ability => ability.OnAbilityEnd(ctx));
        }
    }
}