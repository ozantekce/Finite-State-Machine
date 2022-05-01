using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    private List<MyAction> actions = new List<MyAction>();
    private List<Transition> transitions = new List<Transition>();

    /// <summary>
    /// Here we set the state, actions and transitions should be specified here.
    /// </summary>
    public abstract void Init();

    /// <summary>
    /// This method is called when entering a new state.
    /// </summary>
    /// <param name="fsm"></param>
    public abstract void Enter(FiniteStateMachine fsm);

    /// <summary>
    /// this method is called when exiting the current state
    /// </summary>
    /// <param name="fsm"></param>
    public abstract void Exit(FiniteStateMachine fsm);


    /// <summary>
    /// It's a control value to initialize actions and transitions before first execute
    /// </summary>
    private bool first = true;

    /// <summary>
    /// This method first calls actions then calls transitions
    /// </summary>
    /// <param name="fsm"></param>
    public void Execute(FiniteStateMachine fsm)
    {

        if (first)
        {
            Init();
            first = false;
        }



        foreach (MyAction action in actions)
        {

            action.ExecuteAction(fsm);

        }

        foreach (Transition transition in transitions)
        {
            transition.Decide(fsm);
        }

    }


    /// <summary>
    /// Add an Action
    /// </summary>
    /// <param name="myAction"></param>
    public void AddAction(MyAction myAction)
    {
        if(!actions.Contains(myAction))
            actions.Add(myAction);

    }
    /// <summary>
    /// Add an action
    /// </summary>
    /// <param name="method"></param>
    public void AddAction(MyDelegates.Method method)
    {
        actions.Add(new MyAction(method));
    }

    /// <summary>
    /// Add an action is timed
    /// </summary>
    /// <param name="method"></param>
    /// <param name="waitBefore"></param>
    /// <param name="waitAfter"></param>
    public void AddAction(MyDelegates.Method method,float waitBefore,float waitAfter)
    {
        actions.Add(new MyAction(method,waitBefore,waitAfter));
    }

    /// <summary>
    /// Add an action is conditional
    /// </summary>
    /// <param name="method"></param>
    /// <param name="condition"></param>
    public void AddAction(MyDelegates.Method method, MyDelegates.ConditionMethod condition)
    {
        actions.Add(new MyAction(method, condition));
    }

    /// <summary>
    /// Add an action is conditional and timed
    /// </summary>
    /// <param name="method"></param>
    /// <param name="condition"></param>
    /// <param name="waitBefore"></param>
    /// <param name="waitAfter"></param>
    public void AddAction(MyDelegates.Method method, MyDelegates.ConditionMethod condition, float waitBefore, float waitAfter)
    {
        actions.Add(new MyAction(method, condition,waitBefore,waitAfter));
    }


    /// <summary>
    /// Add an transition
    /// </summary>
    /// <param name="state"></param>
    /// <param name="condition"></param>
    public void AddTransition(State state,MyDelegates.ConditionMethod condition)
    {
        transitions.Add(new Transition(state,condition));
    }


}
