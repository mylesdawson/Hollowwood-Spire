public enum AbilityMutationType
{
    Move,
    Attack,
    Jump,
    Dash
}

// Each ability type can have many ability mutations
public abstract class AbilityMutation
{
    public abstract AbilityType AbilityType { get; }

    /// <summary>
    /// Called when the ability begins
    /// </summary>
    public abstract void OnStart(ActionContext ctx);

    /// <summary>
    /// Called every frame during the ability. Return true if ability should end.
    /// </summary>
    public abstract bool OnUpdate(ActionContext ctx, float dt);

    /// <summary>
    /// Called when the ability ends
    /// </summary>
    public abstract void OnEnd(ActionContext ctx);
}