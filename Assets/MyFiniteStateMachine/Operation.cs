using System;
using System.Collections;
using UnityEngine;

public class Operation
{
    private static CoroutineRunner s_CoroutineRunner;
    private static CoroutineRunner CoroutineRunner
    {
        get
        {
            if (s_CoroutineRunner == null)
            {
                GameObject dummy = new GameObject("CoroutineRunner");
                s_CoroutineRunner = dummy.AddComponent<CoroutineRunner>();
            }
            return s_CoroutineRunner;
        }
    }

    public Action<IStateMachine> Action { get; private set; }
    public Func<IStateMachine, bool> Condition { get; private set; }
    public float WaitBefore { get; private set; } = -1f;
    public float WaitAfter { get; private set; } = -1f;


    public Operation(Action<IStateMachine> action, float waitBefore = -1f, float waitAfter = -1f, Func<IStateMachine, bool> condition = null)
    {
        Action = action;
        WaitBefore = waitBefore;
        WaitAfter = waitAfter;
        Condition = condition;
    }

    public void ExecuteAction(IStateMachine fsm)
    {
        if (!IsActionExecutable(fsm)) return;

        if (IsTimedAction())
            CoroutineRunner.StartCoroutine(TimedAction(fsm));
        else
            Action.Invoke(fsm);
    }

    public bool IsActionOver(IStateMachine fsm)
    {
        return fsm.ActionStatuses.TryGetValue(this, out ActionStatus status) && status != ActionStatus.Running;
    }

    private bool IsActionExecutable(IStateMachine fsm)
    {
        InitActionStatus(fsm, ActionStatus.None);
        if (fsm.ActionStatuses[this] == ActionStatus.Running) return false;
        if (Condition != null && !Condition.Invoke(fsm)) return false;
        return true;
    }

    private bool IsTimedAction()
    {
        return WaitBefore > 0f || WaitAfter > 0f;
    }

    private void InitActionStatus(IStateMachine fsm, ActionStatus defaultStatus)
    {
        if (!fsm.ActionStatuses.ContainsKey(this))
            fsm.ActionStatuses[this] = defaultStatus;
    }

    private IEnumerator TimedAction(IStateMachine fsm)
    {
        fsm.ActionStatuses[this] = ActionStatus.Running;
        if (WaitBefore > 0f) yield return new WaitForSeconds(WaitBefore);
        Action?.Invoke(fsm);
        if (WaitAfter > 0f) yield return new WaitForSeconds(WaitAfter);
        fsm.ActionStatuses[this] = ActionStatus.Over;
    }
}

public enum ActionStatus
{
    None,
    Running,
    Over,
}

public class CoroutineRunner : MonoBehaviour { }
