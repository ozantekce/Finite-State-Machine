using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    private List<MyAction> actions = new List<MyAction>();
    private List<Transition> transitions = new List<Transition>();

    /// <summary>
    /// It's a control value to initialize actions and transitions before first execute
    /// </summary>
    private bool first = true;

    /// <summary>
    /// Here we set the state, actions and transitions should be specified here.
    /// </summary>
    public abstract void Init(FiniteStateMachine fsm);

    /// <summary>
    /// This method is called when entering a new state.
    /// </summary>
    /// <param name="fsm"></param>
    public abstract void Enter(FiniteStateMachine fsm);



    protected virtual void PreExecute(FiniteStateMachine fsm)
    {

    }

    /// <summary>
    /// This method first calls actions then calls transitions
    /// </summary>
    /// <param name="fsm"></param>
    public void Execute(FiniteStateMachine fsm)
    {

        if (first)
        {
            Init(fsm);
            first = false;
        }


        PreExecute(fsm);


        foreach (MyAction action in actions)
        {

            action.ExecuteAction(fsm);

        }

        foreach (Transition transition in transitions)
        {

            if (transition.Decide(fsm))
            {
                //exit
                return;
            }

        }


        PostExecute(fsm);

    }


    protected virtual void PostExecute(FiniteStateMachine fsm)
    {

    }



    /// <summary>
    /// this method is called when exiting the current state
    /// </summary>
    /// <param name="fsm"></param>
    public abstract void Exit(FiniteStateMachine fsm);
    



    /// <summary>
    /// Add an action
    /// </summary>
    /// <param name="method"></param>
    /// <returns></returns>
    public MyAction AddAction(MyDelegates.Method method)
    {
        MyAction temp = new MyAction(method);
        actions.Add(temp);
        return temp;

    }


    /// <summary>
    /// Add an action is timed
    /// </summary>
    /// <param name="method"></param>
    /// <param name="waitBefore"></param>
    /// <param name="waitAfter"></param>
    /// <returns></returns>
    public MyAction AddAction(MyDelegates.Method method,float waitBefore,float waitAfter)
    {
        MyAction temp = new MyAction(method, waitBefore, waitAfter);
        actions.Add(temp);
        return temp;
    }



    /// <summary>
    /// Add an action is conditional
    /// </summary>
    /// <param name="method"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public MyAction AddAction(MyDelegates.Method method, MyDelegates.ConditionMethod condition)
    {
        MyAction temp = new MyAction(method, condition);
        actions.Add(temp);
        return temp;
    }


    /// <summary>
    /// Add an action is conditional and timed
    /// </summary>
    /// <param name="method"></param>
    /// <param name="condition"></param>
    /// <param name="waitBefore"></param>
    /// <param name="waitAfter"></param>
    /// <returns></returns>
    public MyAction AddAction(MyDelegates.Method method, MyDelegates.ConditionMethod condition, float waitBefore, float waitAfter)
    {
        MyAction temp = new MyAction(method,condition,waitBefore,waitAfter);
        actions.Add(temp);
        return temp;

    }


    /// <summary>
    /// Add an transition
    /// </summary>
    /// <param name="state"></param>
    /// <param name="condition"></param>
    /// <returns></returns>
    public Transition AddTransition(State state,MyDelegates.ConditionMethod condition)
    {
        Transition temp = new Transition(state,condition);
        transitions.Add(temp);
        return temp;

    }



}
