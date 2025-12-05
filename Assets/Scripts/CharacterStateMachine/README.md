
# How to use?

- Add StateManager to enemy, npm, player

- call .StartMachine(ctx) on start of Unity MonoBehavior

- call stateManager.UpdateState(ctx) in the Update loop

- Relies heavily on the ActionContext class