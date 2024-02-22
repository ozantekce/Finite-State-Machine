using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is created to control the transition from one state to another.
/// </summary>
public class Transition
{

    private State State { get; set; }
    private Func<IStateMachine, bool> Condition { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="condition"></param>
    public Transition(State state, Func<IStateMachine, bool> condition)
    {
        State = state;
        Condition = condition;
    }

    /// <summary>
    /// If the condition is true, the current state changes.
    /// </summary>
    /// <param name="fsm"></param>
    /// <returns></returns>
    public bool Decide(IStateMachine fsm)
    {

        if (Condition(fsm))
        {
            fsm.ChangeCurrentState(State);
            return true;
        }
        return false;

    }



}

