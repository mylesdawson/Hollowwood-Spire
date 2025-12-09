using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MoveConfig
{
    float baseMoveSpeed;
    float moveThreshold;

    public MoveConfig(float baseMoveSpeed = 5f, float moveThreshold = .3f)
    {   
        this.baseMoveSpeed = baseMoveSpeed;
        this.moveThreshold = moveThreshold;
    }

    public float GetMoveSpeed(List<MoveStatMutation> moveStatMutations)
    {
        float result = baseMoveSpeed;
        var moveSpeedMutations = moveStatMutations.OfType<MoveSpeedMutation>();
        foreach (var mutation in moveSpeedMutations)
        {
            result += mutation.data;
        }
        return result;
    }

    public float GetMoveThreshold()
    {
        return moveThreshold;
    }
}   

public class MoveManager: MonoBehaviour
{
    bool initialized = false;
    MoveConfig config;
    public List<MoveStatMutation> moveStatMutations = new();
    public List<MoveAbilityMutation> moveAbilityMutations = new();

    void Awake()
    {
        // config = new();
        // moveStatMutations.Add(new MoveSpeedMutation(5f));
        // moveAbilityMutations.Add(new PlantSpawnerMove());
    }

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
                ctx.VelocityX = sign * config.GetMoveSpeed(moveStatMutations);
                moveAbilityMutations.ForEach(ability => ability.OnUpdate(ctx, Time.deltaTime));
            }
        }
    }
}