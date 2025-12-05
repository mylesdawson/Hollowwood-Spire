using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RegularAttack : AttackBehavior
{
    Vector2 initialMovementInput;

    public RegularAttack(Transform attackPrefab, List<AttackStatMutation> attackMutations): base(attackMutations)
    {
        this.config = new AttackConfig(attackPrefab);
    }

    public override void OnStart(ActionContext ctx)
    {
        this.attackTimer = config.GetAttackDuration(this.attackMutations);
        // SoundEffectsManager.Instance.PlayEffect("Attack");
        attackablesHitThisAttack.Clear();
        ctx.IsAttacking = true;

        this.initialMovementInput = ctx.MovementInput;

        // TODO: hardcoded distance from player hmmm
        var distance = 2f;
        var directionality = ctx.CurrentDirection;
        config.attackPrefab.gameObject.SetActive(true);
        if (!ctx.IsGrounded && initialMovementInput.y < this.config.downwardAttackInputThreshold)
        {
            config.attackPrefab.position = ctx.Transform.position + new Vector3(0f, -distance, 0f);
            config.attackPrefab.rotation = Quaternion.Euler(0, 0, 270);
        }
        else if (initialMovementInput.y > config.upwardAttackInputThreshold)
        {
            config.attackPrefab.position = ctx.Transform.position + new Vector3(0f, distance, 0f);
            config.attackPrefab.rotation = Quaternion.Euler(0, 0, 90);
        }
        else
        {
            // Facing left
            if (directionality < 0)
            {
                config.attackPrefab.position = ctx.Transform.position + new Vector3(-distance, 0f, 0f);
                config.attackPrefab.rotation = Quaternion.Euler(0, 0, 180);
            }
            // Facing right
            else
            {
                config.attackPrefab.position = ctx.Transform.position + new Vector3(distance, 0f, 0f);
                config.attackPrefab.rotation = Quaternion.identity;
            }
        }

        this.pogoTimer = 0f;
        this.selfKnockbackStrengthTimer = 0f;
    }

    public override bool OnUpdate(ActionContext ctx, float dt)
    {
        var movementInput = ctx.MovementInput;
        var isGrounded = ctx.CurrentState is Grounded;
        var directionality = ctx.CurrentDirection;
        var pressedAttackThisFrame = ctx.DidAttackThisFrame;
        
        // Detect which enemies are hit by this active attack and notify them once per swing
        Collider2D[] hits = Physics2D.OverlapBoxAll(config.attackPrefab.position, config.attackCollider.bounds.size, config.enemyLayer);
        foreach (var hit in hits)
        {
            if (hit == null) continue;
            // Knockback direction: away from player, with a slight upward component
            Vector2 dir = hit.transform.position - ctx.Transform.position;
            if (dir == Vector2.zero) dir = new Vector2(directionality, 0.5f);
            // Attackable hits, apply damage
            if (hit.TryGetComponent<Attackable>(out var attackable))
            {
                int id = attackable.GetInstanceID();
                if (attackablesHitThisAttack.Contains(id)) continue;
                attackablesHitThisAttack.Add(id);
                attackable.TakeDamage(config.GetAttackDamage(this.attackMutations), dir, config.GetEnemyKnockBackStrength(this.attackMutations));
            }
        }

        var attackableArray = hits
            .Where(hit => hit != null && hit.TryGetComponent(out Attackable _))
            .Select(hit => hit.GetComponent<Attackable>())
            .ToArray();
        int numberOfHits = attackableArray.Length;
        if (numberOfHits > 0)
        {
            selfKnockbackStrengthTimer = config.selfKnockbackStrengthDuration;
        }

        // attacking downward and a bounceable
        var maxBounceForce = hits
            .Where(hit => hit != null && hit.TryGetComponent(out Bouncable _))
            .Select(hit => hit.GetComponent<Bouncable>().GetBounceForce())
            .DefaultIfEmpty(0f)
            .Max();
        if (maxBounceForce > 0 && this.initialMovementInput.y < config.downwardAttackInputThreshold && ctx.CurrentState is Airborne)
        {
            pogoTimer = config.pogoTimerDuration;
            selfKnockbackStrengthTimer = 0f;
        }


        if (pogoTimer > 0f)
        {
            ctx.VelocityY = config.GetPogoVelocityY(this.attackMutations);
        }

        if (selfKnockbackStrengthTimer > 0)
        {
            var bonus = -1 * directionality * config.GetSelfKnockBackStrength(this.attackMutations);
            ctx.VelocityX += bonus;
        }

        this.pogoTimer -= dt;
        this.selfKnockbackStrengthTimer -= dt;
        this.attackTimer -= dt;

        return attackTimer > 0f;
    }

    public override void OnEnd(ActionContext ctx)
    {
        config.attackPrefab.gameObject.SetActive(false);
        ctx.IsAttacking = false;
    }

}