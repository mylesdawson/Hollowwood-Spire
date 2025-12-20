using System.Collections.Generic;
using UnityEngine;

public class MoveManager: MonoBehaviour
{
    bool initialized = false;
    MoveConfig config;
    public List<AbilityStatMutation> moveStatMutations = new();
    public List<Ability> moveAbilityMutations = new();

    public void Initialize(MoveConfig config)
    {
        this.config = config;
        initialized = true;
    }

    public void OnUpdate(ActionContext ctx)
    {
        if(!initialized) return;
        if(!ctx.IsHit && !ctx.IsDashing && !ctx.IsWallJumping)
        {
            Vector2 movementInput = ctx.MovementInput;
            var moveThreshold = config.GetMoveThreshold();

            if((movementInput.x < 0 && movementInput.x > -moveThreshold) || (movementInput.x > 0 && movementInput.x < moveThreshold) || movementInput.x == 0) {
                ctx.VelocityX = 0;
            } else
            {
                var sign = movementInput.x > 0 ? 1 : -1;
                ctx.VelocityX = sign * config.GetStat(AbilityStat.moveSpeed, moveStatMutations);
                moveAbilityMutations.ForEach(ability => ability.OnUpdate(ctx, Time.deltaTime, moveStatMutations));
            }
        }
    }
}