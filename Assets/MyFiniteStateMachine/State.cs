using System;
using System.Collections.Generic;

public abstract class State
{

    private bool _firstTime = true;

    public Action OnEnter { get; set; }
    public Action OnUpdate { get; set; }
    public Action OnFixedUpdate { get; set; }
    public Action OnExit { get; set; }

    public Dictionary<Runtime, List<Operation>> RuntimeToActionsDic { get; private set; } = new Dictionary<Runtime, List<Operation>>();
    public Dictionary<Runtime, List<Transition>> RuntimeToTransitionDic { get; private set; } = new Dictionary<Runtime, List<Transition>>();

    private Dictionary<Runtime, List<PriorityWrapper>> RunTimeToPriorityWrapper { get; set; } = new Dictionary<Runtime, List<PriorityWrapper>>();


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
        List<PriorityWrapper> wrappers = RunTimeToPriorityWrapper.GetValueOrDefault(Runtime.Enter, null);
        if (wrappers != null)
        {

            foreach (var wrapper in wrappers)
            {
                if (wrapper.Operation != null)
                {
                    wrapper.Operation.ExecuteAction(fsm);
                }
                else if (wrapper.Transition != null)
                {
                    if (wrapper.Transition.Decide(fsm))
                    {
                        return;
                    }
                }
            }
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


        List<PriorityWrapper> wrappers = RunTimeToPriorityWrapper.GetValueOrDefault(Runtime.Update, null);
        if(wrappers != null)
        {

            foreach (var wrapper in wrappers)
            {
                if(wrapper.Operation != null)
                {
                    wrapper.Operation.ExecuteAction(fsm);
                }
                else if(wrapper.Transition != null)
                {
                    if (wrapper.Transition.Decide(fsm))
                    {
                        return;
                    }
                }
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
        List<PriorityWrapper> wrappers = RunTimeToPriorityWrapper.GetValueOrDefault(Runtime.FixedUpdate, null);
        if (wrappers != null)
        {

            foreach (var wrapper in wrappers)
            {
                if (wrapper.Operation != null)
                {
                    wrapper.Operation.ExecuteAction(fsm);
                }
                else if (wrapper.Transition != null)
                {
                    if (wrapper.Transition.Decide(fsm))
                    {
                        return;
                    }
                }
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
        List<PriorityWrapper> wrappers = RunTimeToPriorityWrapper.GetValueOrDefault(Runtime.Exit, null);
        if (wrappers != null)
        {

            foreach (var wrapper in wrappers)
            {
                if (wrapper.Operation != null)
                {
                    wrapper.Operation.ExecuteAction(fsm);
                }
                else if (wrapper.Transition != null)
                {
                    if (wrapper.Transition.Decide(fsm))
                    {
                        return;
                    }
                }
            }
        }
        OnExit?.Invoke();
    }



    /// <summary>
    /// Add an action that is run on specific sequence
    /// an action can run on enter, on update, on fixedUpdate and on exit.
    /// </summary>
    /// <param name="action"></param>
    /// <param name="runtime"></param>
    public void AddAction(Operation action, Runtime runtime = Runtime.Update, float priority = 1f)
    {
        List<Operation> actions = RuntimeToActionsDic.GetValueOrDefault(runtime, new List<Operation>());
        actions.Add(action);
        RuntimeToActionsDic[runtime] = actions;

        List<PriorityWrapper> priorityWrappers = RunTimeToPriorityWrapper.GetValueOrDefault(runtime, new List<PriorityWrapper>());
        priorityWrappers.Add(new PriorityWrapper(action, priority));
        priorityWrappers.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        RunTimeToPriorityWrapper[runtime] = priorityWrappers;

    }


    /// <summary>
    /// Add a transition that is run on specific sequence
    /// a transition can run on before executing,on executing and after executing.
    /// </summary>
    /// <param name="transition"></param>
    /// <param name="type"></param>
    public void AddTransition(Transition transition, Runtime runtime = Runtime.Update, float priority = 1f)
    {
        List<Transition> transitions = RuntimeToTransitionDic.GetValueOrDefault(runtime, new List<Transition>());
        transitions.Add(transition);
        RuntimeToTransitionDic[runtime] = transitions;

        List<PriorityWrapper> priorityWrappers = RunTimeToPriorityWrapper.GetValueOrDefault(runtime, new List<PriorityWrapper>());
        priorityWrappers.Add(new PriorityWrapper(transition, priority));
        priorityWrappers.Sort((a, b) => b.Priority.CompareTo(a.Priority));
        RunTimeToPriorityWrapper[runtime] = priorityWrappers;
    }


    public List<Operation> RuntimeToActions(Runtime runtime)
    {
        if (!RuntimeToActionsDic.ContainsKey(runtime))
        {
            RuntimeToActionsDic[runtime] = new List<Operation>();
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



    private class PriorityWrapper
    {

        public float Priority { get; private set; } = 1f;
        public Operation Operation { get; private set; }
        public Transition Transition { get; private set; }

        public PriorityWrapper(Operation operation, float priority = 1f)
        {
            Operation = operation;
            Priority = priority;
        }

        public PriorityWrapper(Transition transition, float priority = 1f)
        {
            Transition = transition;
            Priority = priority;
        }

    }


}
public enum Runtime
{
    Enter, Update, FixedUpdate, Exit
}