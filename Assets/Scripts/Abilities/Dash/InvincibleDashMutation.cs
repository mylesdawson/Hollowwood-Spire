using UnityEngine;

public class InvincibleDashMutation  : AbilityMutation
{
    private int originalLayer;
    private const string INVINCIBLE_LAYER = "InvinciblePlayer";

    public override void OnAbilityStart(ActionContext ctx)
    {     
        originalLayer = ctx.Transform.gameObject.layer;
        ctx.Transform.gameObject.layer = LayerMask.NameToLayer(INVINCIBLE_LAYER);
    }

    public override bool OnAbilityUpdate(ActionContext ctx, float dt)
    {
        return false;
    }

    public override void OnAbilityEnd(ActionContext ctx)
    {
        ctx.Transform.gameObject.layer = originalLayer;
    }
}