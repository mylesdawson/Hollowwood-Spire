using System.Collections.Generic;

public enum AbilityType
{
    Move,
    Attack,
    Jump,
    Dash
}


// Each ability type can have 1 currently assigned ability
// Think 'type' of ability. For example, a regular attack vs a heavy attack, etc.
public abstract class Ability
{
    public abstract string AbilityName { get; }
    public abstract string AbilityDescription { get; }
    public abstract AbilityType AbilityType { get; }

    /// <summary>
    /// Called when the ability begins
    /// </summary>
    public abstract void OnStart(ActionContext ctx, List<AbilityStatMutation> statMutations);

    /// <summary>
    /// Called every frame during the ability. Return true if ability should continue, false if it should end.
    /// </summary>
    public abstract bool OnUpdate(ActionContext ctx, float dt, List<AbilityStatMutation> statMutations);

    /// <summary>
    /// Called when the ability ends
    /// </summary>
    public abstract void OnEnd(ActionContext ctx, List<AbilityStatMutation> statMutations);
}