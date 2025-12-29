using UnityEngine;

public abstract class PlayerStateMachine : MonoBehaviour
{
    public Grounded grounded = new();
    public Airborne airborne = new();
    public Walled walled = new();

    public abstract void StartMachine(ActionContext ctx);
    public abstract void UpdateState(ActionContext ctx);

    public abstract void SwitchState(ActionContext ctx, BaseState newState);
}