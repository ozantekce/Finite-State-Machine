using System.Collections.Generic;
using UnityEngine;

public abstract class MonoStateMachine : MonoBehaviour, IStateMachine
{

    public State CurrentState { get; private set; }
    public float EnterTimeCurrentState { get; private set; }
    public Dictionary<Operation, ActionStatus> ActionStatuses { get; set; } = new Dictionary<Operation, ActionStatus>();


    public void Update()
    {
        CurrentState?.Update(this);
    }

    public void FixedUpdate()
    {
        CurrentState?.FixedUpdate(this);
    }


    public void ChangeCurrentState(State state)
    {
        if(CurrentState != null)
            CurrentState.Exit(this);
        
        CurrentState = state;
        CurrentState.Enter(this);
        // reset enter time of current state
        EnterTimeCurrentState = Time.time;


    }

    
    public float ElapsedTimeInCurrentState()
    {
        return Time.time - EnterTimeCurrentState;
    }


}



