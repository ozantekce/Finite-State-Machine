using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is created to control the transition from one state to another.
/// </summary>
public class Transition
{

    private State state;
    private MyDelegates.ConditionMethod condition;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="condition"></param>
    public Transition(State state,MyDelegates.ConditionMethod condition)
    {
        this.state = state;
        this.condition = condition;
    }

    /// <summary>
    /// If the condition is true, the current state changes.
    /// </summary>
    /// <param name="fsm"></param>
    /// <returns></returns>
    public bool Decide(FiniteStateMachine fsm)
    {

        if (condition(fsm))
        {
            fsm.ChangeCurrentState(state);
            return true;
        }
        return false;

    }



}

