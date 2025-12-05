


using Unity.VisualScripting;
using UnityEngine;

public class KnockupAttackMutation : AbilityMutation
{
    bool hasHitTheFloorThisAttack = false;
    public override void OnAbilityStart(ActionContext ctx)
    {
        // throw new System.NotImplementedException();
        hasHitTheFloorThisAttack = false;
    }

    public override bool OnAbilityUpdate(ActionContext ctx, float dt)
    {
        
        // check if current attack hit ground
        var attackMgr = ctx.Transform.GetComponent<AttackManager>();
        var groundLayer = LayerMask.GetMask("Terrain");
        Collider2D[] hits = Physics2D.OverlapBoxAll(attackMgr.attack.config.attackPrefab.position, attackMgr.attack.config.attackCollider.bounds.size, groundLayer);
        foreach (var hit in hits)
        {
            if (hit == null) continue;

            if(hit.gameObject.tag == "Floor")
            {
                if(!hasHitTheFloorThisAttack)
                {
                    ctx.Transform.AddComponent<Knockup>().Initialize(ctx, hit.gameObject);
                }
                hasHitTheFloorThisAttack = true;
            }
        }



        return true;
    }

    public override void OnAbilityEnd(ActionContext ctx)
    {
        // throw new System.NotImplementedException();
    }
}