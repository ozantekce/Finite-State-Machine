using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : State
{


    public override void Init(FiniteStateMachine fsm)
    {

        EnemyFSM enemyFSM = fsm.GetComponent<EnemyFSM>();


        AddTransition(enemyFSM.Idle,PlayerNotClose);

        AddAction(ChasePlayer);


    }



    public override void Enter(FiniteStateMachine fsm)
    {
        Debug.Log("Enter : Enemy Chase State");

    }

    public override void Exit(FiniteStateMachine fsm)
    {
        Debug.Log("Exit : Enemy Chase State");

    }

    public static bool PlayerNotClose(FiniteStateMachine fsm)
    {

        EnemyFSM enemyFSM = fsm.GetComponent<EnemyFSM>();
        float distance = Vector3.Distance(enemyFSM.transform.position, enemyFSM.Player.transform.position);
        return !(distance < 10f);

    }

    public static void ChasePlayer(FiniteStateMachine fsm)
    {

        EnemyFSM enemyFSM = fsm.GetComponent<EnemyFSM>();
        Vector3 target = enemyFSM.Player.transform.position;
        Vector3 directionVector = (target - enemyFSM.transform.position).normalized;
        enemyFSM.GetComponent<Rigidbody>().velocity = directionVector * enemyFSM.Speed;

    }




}
