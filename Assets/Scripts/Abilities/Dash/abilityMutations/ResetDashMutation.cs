using System.Collections.Generic;
using UnityEngine;

public class ResetDashMutation : Ability
{
    public override string AbilityName => "Reset Dash Echo";
    public override string AbilityDescription => "Creates a shadow echo of the player at the start of the dash.";
    public override AbilityType AbilityType => AbilityType.Dash;
    public override AbilitySubtype AbilitySubtype => AbilitySubtype.Mutation;
    GameObject copyOfPlayer;
    Vector3 position;

    public override void OnStart(ActionContext ctx, List<AbilityStatMutation> statMutation)
    {
        position = ctx.Transform.position;
        copyOfPlayer = new GameObject("PlayerPrevPos");
        copyOfPlayer.transform.SetParent(ctx.Transform.parent);
        copyOfPlayer.transform.position = position;
        copyOfPlayer.transform.localScale = ctx.Transform.lossyScale;
        var renderer = copyOfPlayer.AddComponent<SpriteRenderer>();
        renderer.sprite = ctx.SpriteRenderer.sprite;
        renderer.flipX = ctx.SpriteRenderer.flipX;
        renderer.color = Color.black;
        var lingerer = copyOfPlayer.AddComponent<DashEchoLingerer>();
        lingerer.Initialize(ctx, position);
    }

    public override bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> statMutation)
    {
        return false;
    }

    public override void OnEnd(ActionContext ctx, List<AbilityStatMutation> statMutation)
    {
    }
}