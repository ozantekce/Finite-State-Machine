using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{

    //
    private static List<State> ALL_STATES 
        = new List<State>();


    public State()
    {

        ALL_STATES.Add(this);

    }

    /// <summary>
    /// Specifies run sequence of the Action
    /// </summary>
    public enum RunTimeOfAction
    {
        runOnEnter,runOnPreExecution,runOnExecution,runOnPostExecution,runOnExit
    }
    /// <summary>
    /// SSpecifies run sequence of the Transition
    /// </summary>
    public enum RunTimeOfTransition
    {
        runOnPreExecution,runOnExecution,runOnPostExecution
    }


    private List<MyAction> actions = new List<MyAction>();
    private List<Transition> transitions = new List<Transition>();

    private List<MyAction> enterActions = new List<MyAction>();
    private List<MyAction> exitActions = new List<MyAction>();

    private List<MyAction>  preActions = new List<MyAction>();
    private List<Transition> preTransitions = new List<Transition>();


    private List<MyAction> postActions = new List<MyAction>();
    private List<Transition> postTransitions = new List<Transition>();


    /// <summary>
    /// This method is called when entering a new state and calls enterActions
    /// </summary>
    /// <param name="fsm"></param>
    public void Enter(FiniteStateMachine fsm)
    {

        if (first)
        {
            Init();
            first = false;
        }

        EnterOptional(fsm);

        foreach (MyAction action in enterActions)
        {
            action.ExecuteAction(fsm);
        }

        


    }
    /// <summary>
    /// This method called before Execute and calls preActions and preTransitions
    /// </summary>
    /// <param name="fsm"></param>
    private void PreExecute(FiniteStateMachine fsm)
    {

        PreExecuteOptional(fsm);

        foreach (MyAction action in preActions)
        {

            action.ExecuteAction(fsm);

        }

        foreach (Transition transition in preTransitions)
        {

            if (transition.Decide(fsm))
            {
                //exit
                return;
            }

        }

        

    }
    
    /// <summary>
    /// This method first calls actions then calls transitions
    /// </summary>
    /// <param name="fsm"></param>
    public void Execute(FiniteStateMachine fsm)
    {

        PreExecute(fsm);

        ExecuteOptional(fsm);

        
        // add blocking until all states execute preExecute

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

        // add blocking until all states execute 

        PostExecute(fsm);

    }
    /// <summary>
    /// This method called after Execute and calls postActions and postTransitions
    /// </summary>
    /// <param name="fsm"></param>
    private void PostExecute(FiniteStateMachine fsm)
    {

        PostExecuteOptional(fsm);

        foreach (MyAction action in postActions)
        {

            action.ExecuteAction(fsm);

        }

        foreach (Transition transition in postTransitions)
        {

            if (transition.Decide(fsm))
            {
                //exit
                return;
            }

        }

        

    }

    /// <summary>
    /// this method is called when exiting the current state and calls exitActions
    /// </summary>
    /// <param name="fsm"></param>
    public void Exit(FiniteStateMachine fsm)
    {

        foreach (MyAction action in exitActions)
        {
            action.ExecuteAction(fsm);
        }

        ExitOptional(fsm);
    }



    /// <summary>
    /// It's a control value to initialize actions and transitions before first execute ( I added this for singleton)
    /// </summary>
    private bool first = true;

    /// <summary>
    /// Here we set the state, actions and transitions should be specified here.
    /// </summary>
    public abstract void Init();


    /// <summary>
    /// Optional Enter this will called before Enter method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void EnterOptional(FiniteStateMachine fsm)
    {

    }
    /// <summary>
    /// Optional PreExecute this will called before PreExecute method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void PreExecuteOptional(FiniteStateMachine fsm)
    {

    }
    /// <summary>
    /// Optional Execute this will called before Execute method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void ExecuteOptional(FiniteStateMachine fsm)
    {


    }

    /// <summary>
    /// Optional PostExecute this will called before PostExecute method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void PostExecuteOptional(FiniteStateMachine fsm)
    {

    }

    /// <summary>
    /// Optional Exit this will called before Exit method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void ExitOptional(FiniteStateMachine fsm)
    {

    }






    /// <summary>
    /// Add an action
    /// </summary>
    /// <param name="action"></param>
    public void AddAction(MyAction action)
    {
        actions.Add(action);
    }

    /// <summary>
    /// Add an action that is run on specific sequence
    /// an action can run on entering, before executing, on executing , after executing and exiting.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="type"></param>
    public void AddAction(MyAction action , RunTimeOfAction type)
    {
        if(type == RunTimeOfAction.runOnEnter)
        {
            enterActions.Add(action);
        }
        else if(type == RunTimeOfAction.runOnPreExecution)
        {
            preActions.Add(action);
        }
        else if (type == RunTimeOfAction.runOnExecution)
        {
            actions.Add(action);
        }
        else if (type == RunTimeOfAction.runOnPostExecution)
        {
            postActions.Add(action);
        }
        else if (type == RunTimeOfAction.runOnExit)
        {
            exitActions.Add(action);
        }

    }


    /// <summary>
    /// Add a transition
    /// </summary>
    /// <param name="transition"></param>
    public void AddTransition(Transition transition)
    {
        transitions.Add(transition);
    }

    /// <summary>
    /// Add a transition that is run on specific sequence
    /// a transition can run on before executing,on executing and after executing.
    /// </summary>
    /// <param name="transition"></param>
    /// <param name="type"></param>
    public void AddTransition(Transition transition, RunTimeOfTransition type)
    {
        if (type == RunTimeOfTransition.runOnPreExecution)
        {
            preTransitions.Add(transition);
        }
        else if (type == RunTimeOfTransition.runOnExecution)
        {
            transitions.Add(transition);
        }
        else if (type == RunTimeOfTransition.runOnPostExecution)
        {
            postTransitions.Add(transition);
        }

    }


}
