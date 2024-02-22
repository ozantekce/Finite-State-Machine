using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State
{


    public override void Init()
    {


    }



    protected override void EnterOptional(IStateMachine fsm)
    {
        Debug.Log("Enter : Enemy Chase State");

    }

    protected override void ExitOptional(IStateMachine fsm)
    {
        Debug.Log("Exit : Enemy Chase State");

    }






}
