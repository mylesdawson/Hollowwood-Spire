using System.Collections.Generic;
using UnityEngine;

public class InvincibleDashMutation : Ability
{
    public override string AbilityName => "Invincible Dash";
    public override string AbilityDescription => "During the dash, the player is invincible to damage.";
    public override AbilityType AbilityType => AbilityType.Dash;
    public override AbilitySubtype AbilitySubtype => AbilitySubtype.Mutation;
    private int originalLayer;
    private const string INVINCIBLE_LAYER = "InvinciblePlayer";
    Color originalColor;

    Color invincibleColor = Color.grey;

    public override void OnStart(ActionContext ctx, List<AbilityStatMutation> statMutation)
    {     
        originalLayer = ctx.Transform.gameObject.layer;
        ctx.Transform.gameObject.layer = LayerMask.NameToLayer(INVINCIBLE_LAYER);

        // todo also apply shadow FX

        if(ctx.Transform.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            originalColor = spriteRenderer.color;
            spriteRenderer.color = invincibleColor;
        }
    }

    public override bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> statMutation)
    {
        return false;
    }

    public override void OnEnd(ActionContext ctx, List<AbilityStatMutation> statMutation)
    {
        ctx.Transform.gameObject.layer = originalLayer;
        if(ctx.Transform.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
        {
            spriteRenderer.color = originalColor;
        }
    }
}