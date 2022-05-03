using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FiniteStateMachine : MonoBehaviour
{

    private State currentState;


    // false : running | true : over
    private Dictionary<MyAction,bool> actionData = new Dictionary<MyAction, bool>();

    public State CurrentState { get => currentState; set => currentState = value; }


    private void FixedUpdate()
    {
        if (CurrentState != null)
        {
            currentState.Execute(this);
        }
    }

    public void ChangeCurrentState(State state)
    {
        
        CurrentState.Exit(this);
        CurrentState = state;
        CurrentState.Enter(this);

    }

    public void StopAllTimedActions()
    {
        StopAllCoroutines();
    }

    public void StopTimedAction(IEnumerator enumerator)
    {
        StopCoroutine(enumerator);
    }


    public bool ActionOver(MyAction action)
    {
        if (actionData.ContainsKey(action))
        {
            return actionData[action];
        }
        else
        {
            return false;
        }

    }


    public void ExecuteAction(MyAction action)
    {
        if (action.Timed==MyAction.TIMED.Yes)
        {

            if (!actionData.ContainsKey(action))
            {
                actionData.Add(action, true);
            }

            if (actionData[action] == true)
            {

                if (action.Conditional == MyAction.CONDITIONAL.Yes)
                {
                    if (action.Condition(this))
                    {
                        StartCoroutine(TimedAction(action));
                    }
                }
                else
                {
                    StartCoroutine(TimedAction(action));
                }
                

            }

        }
        else
        {
            if(action.Conditional == MyAction.CONDITIONAL.Yes)
            {
                if (action.Condition(this))
                {
                    action.Method(this);
                }
            }
            else
            {
                action.Method(this);
            }
            

        }

    }






    public IEnumerator TimedAction(MyAction action)
    {
        Debug.Log("hi");
        actionData[action] = false;//running

        yield return new WaitForSeconds(action.WaitBefore);


        action.Method(this);


        yield return new WaitForSeconds(action.WaitAfter);

        actionData[action] = true;//over

    }



}



