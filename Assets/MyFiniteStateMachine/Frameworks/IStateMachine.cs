using System.Collections.Generic;

public interface IStateMachine
{

    public State CurrentState { get; }

    public Dictionary<Operation, ActionStatus> ActionStatuses { get; }

    public float EnterTimeCurrentState { get; }

    public void Update();
    public void FixedUpdate();
    public void ChangeCurrentState(State state);
    public float ElapsedTimeInCurrentState();


}
