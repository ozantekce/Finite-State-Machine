using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{

    public override void Init()
    {

    }


    protected override void EnterOptional(IStateMachine fsm)
    {
        Debug.Log("Enter : Enemy Idle State");
    }

    protected override void ExitOptional(IStateMachine fsm)
    {
        Debug.Log("Exit : Enemy Idle State");
    }







}
