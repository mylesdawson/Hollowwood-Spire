using UnityEngine;

public class ResetDashMutation : AbilityMutation
{
    public override AbilityType AbilityType => AbilityType.Dash;
    GameObject copyOfPlayer;
    Vector3 position;

    public override void OnStart(ActionContext ctx)
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

    public override bool OnUpdate(ActionContext ctx, float dt)
    {
        return false;
    }

    public override void OnEnd(ActionContext ctx)
    {
    }
}