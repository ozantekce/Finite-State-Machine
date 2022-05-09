using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{

    public override void Init()
    {

    }


    protected override void EnterOptional(FiniteStateMachine fsm)
    {
        Debug.Log("Enter : Enemy Idle State");
    }

    protected override void ExitOptional(FiniteStateMachine fsm)
    {
        Debug.Log("Exit : Enemy Idle State");
    }







}
