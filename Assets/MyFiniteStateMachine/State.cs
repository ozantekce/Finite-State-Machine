using System;
using System.Collections.Generic;

public abstract class State
{

    private bool _firstTime = true;

    public Action OnEnter { get; set; }
    public Action OnUpdate { get; set; }
    public Action OnFixedUpdate { get; set; }
    public Action OnExit { get; set; }

    public Dictionary<Runtime, List<MyAction>> RuntimeToActionsDic { get; private set; } = new Dictionary<Runtime, List<MyAction>>();
    public Dictionary<Runtime, List<Transition>> RuntimeToTransitionDic { get; private set; } = new Dictionary<Runtime, List<Transition>>();



    /// <summary>
    /// Here we set the state, actions and transitions should be specified here.
    /// </summary>
    public abstract void Init();


    /// <summary>
    /// This method is called when entering a new state and calls enterActions
    /// </summary>
    /// <param name="fsm"></param>
    public void Enter(IStateMachine fsm)
    {
        if (_firstTime)
        {
            Init();
            _firstTime = false;
        }

        EnterOptional(fsm);
        List<MyAction> actions = RuntimeToActions(Runtime.Enter);
        foreach (MyAction action in actions)
        {
            action.ExecuteAction(fsm);
        }
        OnEnter?.Invoke();
    }

    
    /// <summary>
    /// This method first calls actions then calls transitions
    /// </summary>
    /// <param name="fsm"></param>
    public void Update(IStateMachine fsm)
    {

        UpdateOptional(fsm);
        List<MyAction> actions = RuntimeToActions(Runtime.Update);
        foreach (MyAction action in actions)
        {
            action.ExecuteAction(fsm);
        }

        List<Transition> transitions = RuntimeToTransitions(Runtime.Update);
        foreach (Transition transition in transitions)
        {
            if (transition.Decide(fsm))
            {
                return;
            }
        }

        OnUpdate?.Invoke();
    }

    /// <summary>
    /// This method first calls actions then calls transitions
    /// </summary>
    /// <param name="fsm"></param>
    public void FixedUpdate(IStateMachine fsm)
    {

        FixedUpdateOptional(fsm);
        List<MyAction> actions = RuntimeToActions(Runtime.FixedUpdate);
        foreach (MyAction action in actions)
        {
            action.ExecuteAction(fsm);
        }

        List<Transition> transitions = RuntimeToTransitions(Runtime.FixedUpdate);
        foreach (Transition transition in transitions)
        {
            if (transition.Decide(fsm))
            {
                return;
            }
        }

        OnFixedUpdate?.Invoke();
    }



    /// <summary>
    /// this method is called when exiting the current state and calls exitActions
    /// </summary>
    /// <param name="fsm"></param>
    public void Exit(IStateMachine fsm)
    {
        ExitOptional(fsm);
        List<MyAction> actions = RuntimeToActions(Runtime.Exit);
        foreach (MyAction action in actions)
        {
            action.ExecuteAction(fsm);
        }
        OnExit?.Invoke();
    }



    /// <summary>
    /// Add an action that is run on specific sequence
    /// an action can run on enter, on update, on fixedUpdate and on exit.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="runtime"></param>
    public void AddAction(MyAction action, Runtime runtime = Runtime.Update)
    {
        List<MyAction> actions = RuntimeToActionsDic.GetValueOrDefault(runtime, new List<MyAction>());
        actions.Add(action);
        actions.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        RuntimeToActionsDic[runtime] = actions;
    }


    /// <summary>
    /// Add a transition that is run on specific sequence
    /// a transition can run on before executing,on executing and after executing.
    /// </summary>
    /// <param name="transition"></param>
    /// <param name="type"></param>
    public void AddTransition(Transition transition, Runtime runtime = Runtime.Update)
    {
        List<Transition> transitions = RuntimeToTransitionDic.GetValueOrDefault(runtime, new List<Transition>());
        transitions.Add(transition);
        RuntimeToTransitionDic[runtime] = transitions;
    }


    public List<MyAction> RuntimeToActions(Runtime runtime)
    {
        if (!RuntimeToActionsDic.ContainsKey(runtime))
        {
            RuntimeToActionsDic[runtime] = new List<MyAction>();
        }
        return RuntimeToActionsDic[runtime];
    }

    public List<Transition> RuntimeToTransitions(Runtime runtime)
    {
        if (!RuntimeToTransitionDic.ContainsKey(runtime))
        {
            RuntimeToTransitionDic[runtime] = new List<Transition>();
        }
        return RuntimeToTransitionDic[runtime];
    }


    #region Virtual Methods

    /// <summary>
    /// Optional Enter this will called before Enter method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void EnterOptional(IStateMachine fsm)
    {

    }

    /// <summary>
    /// Optional Update this will called before Update method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void UpdateOptional(IStateMachine fsm)
    {


    }


    /// <summary>
    /// Optional FixedUpdate this will called before FixedUpdate method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void FixedUpdateOptional(IStateMachine fsm)
    {


    }

    /// <summary>
    /// Optional Exit this will called before Exit method
    /// </summary>
    /// <param name="fsm"></param>
    protected virtual void ExitOptional(IStateMachine fsm)
    {

    }


    #endregion



}
public enum Runtime
{
    Enter, Update, FixedUpdate, Exit
}