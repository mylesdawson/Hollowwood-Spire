using System.Collections.Generic;

public enum AbilityType
{
    Move,
    Attack,
    Jump,
    Dash
}


// Each ability type can have 1 currently assigned ability
public abstract class Ability
{
    public abstract AbilityType AbilityType { get; }

    /// <summary>
    /// Called when the ability begins
    /// </summary>
    public abstract void OnStart(ActionContext ctx, List<AbilityStatMutation> statMutations);

    /// <summary>
    /// Called every frame during the ability. Return true if ability should end.
    /// </summary>
    public abstract bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> statMutations);

    /// <summary>
    /// Called when the ability ends
    /// </summary>
    public abstract void OnEnd(ActionContext ctx, List<AbilityStatMutation> statMutations);
}