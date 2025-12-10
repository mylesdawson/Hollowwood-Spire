

using UnityEngine;

class PlantSpawnerMove : AbilityMutation
{
    float spawnRate = 1f;
    float currentTimer = 0f;

    public override AbilityType AbilityType => AbilityType.Move;

    public override void OnEnd(ActionContext ctx)
    {
    }

    public override void OnStart(ActionContext ctx)
    {
    }

    public override bool OnUpdate(ActionContext ctx, float dt)
    {
        if(currentTimer >= spawnRate && ctx.IsGrounded)
        {
            // spawn the plant/bomb etc
            var plant = Resources.Load("Plant1_0");
            Object.Instantiate(plant, ctx.Transform.position, ctx.Transform.rotation);

            currentTimer = 0f;
        }

        currentTimer += dt;

        return false;
    }
}