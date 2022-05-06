using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAction
{

    private MyDelegates.Method method;
    private MyDelegates.ConditionMethod condition;
    private float waitBefore;
    private float waitAfter;
    private TIMED timed;
    private CONDITIONAL conditional;

    /// <summary>
    /// Create an action
    /// </summary>
    /// <param name="method"></param>
    public MyAction(MyDelegates.Method method)
    {
        this.method = method;
        timed = TIMED.No;
        conditional = CONDITIONAL.No;
    }

    /// <summary>
    /// Create an action is timed
    /// </summary>
    /// <param name="method"></param>
    /// <param name="waitBefore"></param>
    /// <param name="waitAfter"></param>
    public MyAction(MyDelegates.Method method,float waitBefore,float waitAfter)
    {

        this.method=method;
        this.waitBefore = waitBefore;
        this.waitAfter = waitAfter;
        timed = TIMED.Yes;
        conditional = CONDITIONAL.No;
    }


    /// <summary>
    /// Create an action is conditional
    /// </summary>
    /// <param name="method"></param>
    /// <param name="condition"></param>
    public MyAction(MyDelegates.Method method,MyDelegates.ConditionMethod condition)
    {
        this.method = method;
        this.condition = condition;
        timed = TIMED.No;
        conditional = CONDITIONAL.Yes;
    }

    /// <summary>
    /// Create an action is conditional and timed
    /// </summary>
    /// <param name="method"></param>
    /// <param name="condition"></param>
    /// <param name="waitBefore"></param>
    /// <param name="waitAfter"></param>
    public MyAction(MyDelegates.Method method, MyDelegates.ConditionMethod condition, float waitBefore, float waitAfter)
    {
        this.method = method;
        this.condition = condition;
        this.waitBefore = waitBefore;
        this.waitAfter=waitAfter;
        timed = TIMED.Yes;
        conditional = CONDITIONAL.Yes;

    }




    public void ExecuteAction(FiniteStateMachine fsm)
    {

        Dictionary<MyAction, bool> actionData = fsm.ActionData;
        MyAction action = this;

        if (action.Timed == MyAction.TIMED.Yes)
        {

            if (!actionData.ContainsKey(action))
            {
                actionData.Add(action, true);
            }

            if (actionData[action] == true)
            {

                if (action.Conditional == MyAction.CONDITIONAL.Yes)
                {
                    if (action.Condition(fsm))
                    {
                        fsm.StartCoroutine(TimedAction(fsm));
                    }
                }
                else
                {
                    fsm.StartCoroutine(TimedAction(fsm));
                }


            }

        }
        else
        {
            if (action.Conditional == MyAction.CONDITIONAL.Yes)
            {
                if (action.Condition(fsm))
                {
                    action.Method(fsm);
                }
            }
            else
            {
                action.Method(fsm);
            }


        }

    }



    /// <summary>
    /// returns true if the action is over
    /// </summary>
    /// <param name="fsm"></param>
    /// <returns></returns>
    public bool ActionOver(FiniteStateMachine fsm)
    {
        Dictionary<MyAction, bool> actionData = fsm.ActionData;
        if (actionData.ContainsKey(this))
        {
            return actionData[this];
        }
        else
        {
            return false;
        }

    }



    private IEnumerator TimedAction(FiniteStateMachine fsm)
    {
        //Debug.Log("hi");
        Dictionary<MyAction, bool> actionData = fsm.ActionData;
        actionData[this] = false;//running

        yield return new WaitForSeconds(this.WaitBefore);


        this.Method(fsm);


        yield return new WaitForSeconds(this.WaitAfter);

        actionData[this] = true;//over

    }



    public TIMED Timed { get => timed; }
    public MyDelegates.Method Method { get => method; set => method = value; }
    public float WaitBefore { get => waitBefore; set => waitBefore = value; }
    public float WaitAfter { get => waitAfter; set => waitAfter = value; }
    public CONDITIONAL Conditional { get => conditional; }
    public MyDelegates.ConditionMethod Condition { get => condition; set => condition = value; }

    public enum TIMED
    {
        No,Yes
    }

    public enum CONDITIONAL
    {
        No,Yes
    }



}
