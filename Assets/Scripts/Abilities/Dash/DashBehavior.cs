
using System.Collections.Generic;

public abstract class DashBehavior
{
    public float currentDashTimer;
    public DashConfig dashConfig;
    protected List<DashStatMutation> dashMutations;

    protected DashBehavior(List<DashStatMutation> dashMutations)
    {
        this.dashMutations = dashMutations;
    }

    /// <summary>
    /// Called when the dash state begins
    /// </summary>
    public abstract void OnDashStart(ActionContext ctx);

    /// <summary>
    /// Called every frame during the dash. Return true if dash should end.
    /// </summary>
    public abstract bool UpdateDash(ActionContext ctx, float dt);

    /// <summary>
    /// Called when the dash state ends
    /// </summary>
    public abstract void OnDashEnd(ActionContext ctx);
}