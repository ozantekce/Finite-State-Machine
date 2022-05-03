using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : State
{

    public override void Init(FiniteStateMachine fsm)
    {
            
        EnemyFSM enemyFSM = fsm.GetComponent<EnemyFSM>();
        AddTransition(enemyFSM.Chase,PlayerClose);

    }



    public override void Enter(FiniteStateMachine fsm)
    {
        Debug.Log("Enter : Enemy Idle State");
    }

    public override void Exit(FiniteStateMachine fsm)
    {
        Debug.Log("Exit : Enemy Idle State");
    }



    public bool PlayerClose(FiniteStateMachine fsm)
    {

        EnemyFSM enemyFSM = fsm.GetComponent<EnemyFSM>();
        float distance = Vector3.Distance(enemyFSM.transform.position, enemyFSM.Player.transform.position);
        return distance < 10f;

    }



}
