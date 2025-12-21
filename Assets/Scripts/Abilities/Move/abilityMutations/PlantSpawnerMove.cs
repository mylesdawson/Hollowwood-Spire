

using System.Collections.Generic;
using UnityEngine;

// This is just an example but doesn't really do anything
class PlantSpawnerMove : Ability
{
    public override string AbilityName => "Plant Spawner Move";
    public override string AbilityDescription => "Spawns plants periodically while moving on the ground.";
    float spawnRate = 1f;
    float currentTimer = 0f;

    public override AbilityType AbilityType => AbilityType.Move;
    public override AbilitySubtype AbilitySubtype => AbilitySubtype.Mutation;

    public override void OnEnd(ActionContext ctx, List<AbilityStatMutation> statMutation)
    {
    }

    public override void OnStart(ActionContext ctx, List<AbilityStatMutation> statMutation)
    {
    }

    public override bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> statMutation)
    {
        if(currentTimer >= spawnRate && ctx.IsGrounded)
        {
            // spawn the plant/bomb etc
            var plant = Resources.Load("Fern_0");
            Object.Instantiate(plant, ctx.Transform.position, ctx.Transform.rotation);

            currentTimer = 0f;
        }

        currentTimer += dt;

        return false;
    }
}