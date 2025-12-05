public abstract class AbilityMutation
{
    /// <summary>
    /// Called when the ability begins
    /// </summary>
    public abstract void OnAbilityStart(ActionContext ctx);

    /// <summary>
    /// Called every frame during the ability. Return true if ability should end.
    /// </summary>
    public abstract bool OnAbilityUpdate(ActionContext ctx, float dt);

    /// <summary>
    /// Called when the ability ends
    /// </summary>
    public abstract void OnAbilityEnd(ActionContext ctx);
}