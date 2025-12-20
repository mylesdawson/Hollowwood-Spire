using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KnockupDownDashMutation : Ability
{
    public override string AbilityName => "Knockup Down Dash";
    public override string AbilityDescription => "Dashing downwards and hitting the ground knocks enemies up into the air.";
    public override AbilityType AbilityType => AbilityType.Dash;
    public override AbilitySubtype AbilitySubtype => AbilitySubtype.Mutation;
    bool didHitTheFloorWithDash = false;
    Vector2 moveInput;
    PlayerController player;
    ActionContext ctx;


    public override void OnStart(ActionContext ctx, List<AbilityStatMutation> statMutation)
    {
        didHitTheFloorWithDash = false;
        moveInput = ctx.MovementInput;
        player = ctx.Transform.GetComponent<PlayerController>();
        player.OnCollideWithEnemy += OnCollideWithEnemy;
        this.ctx = ctx;
    }

    private void OnCollideWithEnemy(Collision2D d)
    {
        if(didHitTheFloorWithDash) return;
        Debug.Log("collided with enemy during dash");
        Debug.Log(d.gameObject);
        if(d.gameObject.CompareTag("Floor") && moveInput.y < 0)
        {
            ctx.Transform.AddComponent<KnockupEffect>().Initialize(ctx, d.gameObject);
            didHitTheFloorWithDash = true;
        }
    }

    public override bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> statMutation)
    {
        return true;
    }

    public override void OnEnd(ActionContext ctx, List<AbilityStatMutation> statMutation)
    {
        player.OnCollideWithEnemy -= OnCollideWithEnemy;
    }
}