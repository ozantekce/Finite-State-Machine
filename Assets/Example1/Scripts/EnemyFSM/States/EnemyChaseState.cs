using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State
{


    public override void Init()
    {


    }



    protected override void Enter_(FiniteStateMachine fsm)
    {
        Debug.Log("Enter : Enemy Chase State");

    }

    protected override void Exit_(FiniteStateMachine fsm)
    {
        Debug.Log("Exit : Enemy Chase State");

    }






}
