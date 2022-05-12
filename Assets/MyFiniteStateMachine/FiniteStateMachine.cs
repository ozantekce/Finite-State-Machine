using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FiniteStateMachine : MonoBehaviour
{

    private State currentState;


    // false : running | true : over
    /// <summary>
    /// keeps information of actions
    /// </summary>
    private Dictionary<MyAction,bool> actionData = new Dictionary<MyAction, bool>();

    public State CurrentState { get => currentState; }
    public float EnterTimeCurrentState { get => enterTimeCurrentState; }
    public Dictionary<MyAction, bool> ActionData { get => actionData; set => actionData = value; }

    private float enterTimeCurrentState;



    public void FixedUpdate()
    {
        if (CurrentState != null)
        {
            currentState.Execute(this);
        }
    }


    public void ChangeCurrentState(State state)
    {
        if(currentState != null)
            CurrentState.Exit(this);
        
        currentState = state;
        CurrentState.Enter(this);
        // reset enter time of current state
        enterTimeCurrentState = Time.time;


    }

    
    public float ElapsedTimeInCurrentState()
    {
        return Time.time - enterTimeCurrentState;
    }
    


}



