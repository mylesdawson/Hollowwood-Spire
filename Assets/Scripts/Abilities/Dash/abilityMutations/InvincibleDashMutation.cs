using UnityEngine;

public class InvincibleDashMutation  : AbilityMutation
{
    public override AbilityType AbilityType => AbilityType.Dash;
    private int originalLayer;
    private const string INVINCIBLE_LAYER = "InvinciblePlayer";


    public override void OnStart(ActionContext ctx)
    {     
        originalLayer = ctx.Transform.gameObject.layer;
        ctx.Transform.gameObject.layer = LayerMask.NameToLayer(INVINCIBLE_LAYER);
    }

    public override bool OnUpdate(ActionContext ctx, float dt)
    {
        return false;
    }

    public override void OnEnd(ActionContext ctx)
    {
        ctx.Transform.gameObject.layer = originalLayer;
    }
}