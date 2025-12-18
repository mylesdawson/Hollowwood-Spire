using Unity.VisualScripting;
using UnityEngine;

public class KnockupDownDashMutation : AbilityMutation
{
    public override AbilityType AbilityType => AbilityType.Dash;
    bool didHitTheFloorWithDash = false;
    Vector2 moveInput;
    PlayerController player;


    public override void OnStart(ActionContext ctx)
    {
        didHitTheFloorWithDash = false;
        moveInput = ctx.MovementInput;
        player = ctx.Transform.GetComponent<PlayerController>();
    }

    public override bool OnUpdate(ActionContext ctx, float dt)
    {
        // check if player hit the ground while dashing downwards
        if(!didHitTheFloorWithDash && player)
        {
            if(player.IsGrounded() && moveInput.y < 0)
            {
                var groundLayer = LayerMask.GetMask("MainGround");
                Collider2D[] hits = Physics2D.OverlapBoxAll(ctx.Transform.position, player.bodyCollider.bounds.size, 0f, groundLayer);
                foreach (var hit in hits)
                {
                    if (hit == null) continue;
                    if(hit.gameObject.CompareTag("Floor"))
                    {
                        if(!didHitTheFloorWithDash)
                        {
                            ctx.Transform.AddComponent<KnockupEffect>().Initialize(ctx, hit.gameObject);
                        }
                        didHitTheFloorWithDash = true;
                    }
                }
            }
        }
        return true;
    }

    public override void OnEnd(ActionContext ctx)
    {
    }
}