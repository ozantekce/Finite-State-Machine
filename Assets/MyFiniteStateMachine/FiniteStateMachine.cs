using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FiniteStateMachine : MonoBehaviour
{

    private State currentState;


    // false : running | true : over
    private Dictionary<MyAction,bool> actionData = new Dictionary<MyAction, bool>();

    public State CurrentState { get => currentState; set => currentState = value; }
    public float EnterTimeCurrentState { get => enterTimeCurrentState; }
    public Dictionary<MyAction, bool> ActionData { get => actionData; set => actionData = value; }

    private float enterTimeCurrentState;



    private void FixedUpdate()
    {
        if (CurrentState != null)
        {
            currentState.Execute(this);
        }
    }


    public void ChangeCurrentState(State state)
    {
        
        CurrentState.Exit(this);
        CurrentState = state;
        CurrentState.Enter(this);
        // reset enter time to current state
        enterTimeCurrentState = Time.time;


    }

    
    public float ElapsedTimeInCurrentState()
    {
        return Time.time - enterTimeCurrentState;
    }
    


}



