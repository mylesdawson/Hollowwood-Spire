


using UnityEngine;

public class StateManager: GameStateMachine
{
    public BaseState currentState;
    public BaseState previousState;

    public override void StartMachine(ActionContext ctx)
    {
        currentState = grounded;
        currentState.OnEnter(ctx, this);    
    }

    public override void UpdateState(ActionContext ctx)
    {
        currentState.UpdateState(ctx, this, Time.deltaTime);
    }

    public override void SwitchState(ActionContext ctx, BaseState newState)
    {
        Debug.Log("Changing state from: " +  currentState.GetType().Name + " to: " + newState.GetType().Name);
        previousState = currentState;
        currentState.OnExit(ctx, this);
        currentState = newState;
        currentState.OnEnter(ctx, this);
    }
}