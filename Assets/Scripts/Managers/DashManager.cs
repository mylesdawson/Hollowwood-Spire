

using System.Collections.Generic;
using UnityEngine;

public class DashManager: MonoBehaviour
{
    bool initialized = false;
    public DashBehavior dashAbility;
    public List<AbilityStatMutation> dashStatMutations = new();
    public List<Ability> dashAbilityMutations = new();

    void Awake()
    {
        // dashStatMutations.Add(new DashSpeedMutation(2f));
        // dashStatMutations.Add(new NumDashesMutation(1));
        // dashAbilityMutations.Add(new ResetDashMutation());
        // dashAbilityMutations.Add(new InvincibleDashMutation());
        // dashAbilityMutations.Add(new KnockupDownDashMutation());
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
        dashAbility.OnStart(ctx, dashStatMutations);
        this.dashAbilityMutations.ForEach(ability => ability.OnStart(ctx, dashStatMutations));
    }

    void UpdateDash(ActionContext ctx, float dt)
    {
        bool shouldContinueDashing = dashAbility.OnUpdate(ctx, dt, dashStatMutations);
        this.dashAbilityMutations.ForEach(ability => ability.OnUpdate(ctx, dt, dashStatMutations));
        if(!shouldContinueDashing)
        {
            ctx.IsDashing = false;
            dashAbility?.OnEnd(ctx, dashStatMutations);
            this.dashAbilityMutations.ForEach(ability => ability.OnEnd(ctx, dashStatMutations));
            if(ctx.IsGrounded)
            {
                dashAbility.dashConfig.ResetRemainingDashes(dashStatMutations);
            }
        }
    }

    public List<Ability> GetAbilities()
    {
        List<Ability> abilities = new()
        {
            dashAbility
        };
        abilities.AddRange(dashAbilityMutations);
        return abilities;
    }

    public void AddAbility(Ability ability)
    {
        if(ability.AbilitySubtype == AbilitySubtype.Mutation)
        {
            dashAbilityMutations.Add(ability);
        } else
        {
            dashAbility = (DashBehavior)ability;
        }
    }
}