using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoStateMachine
{

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float m_Speed;

    private EnemyIdleState idle;
    private EnemyChaseState chase;

    public EnemyIdleState Idle { get => idle; set => idle = value; }
    public EnemyChaseState Chase { get => chase; set => chase = value; }
    public Transform Player { get => player; set => player = value; }
    public float Speed { get => m_Speed; set => m_Speed = value; }

    private void Start()
    {

        idle = new EnemyIdleState();
        chase = new EnemyChaseState();
        ChangeCurrentState(idle);

        idle.AddTransition(new Transition(chase, PlayerClose));


        chase.AddTransition(new Transition(idle, PlayerNotClose));



        chase.AddAction(new Operation(ChasePlayer));
        
        chase.AddAction(new Operation((fsm) => {
            Debug.Log("hi player");
        }),Runtime.Enter);




    }
    public bool PlayerClose(IStateMachine fsm)
    {
        MonoStateMachine monoStateMachine = fsm as MonoStateMachine;
        EnemyFSM enemyFSM = monoStateMachine.GetComponent<EnemyFSM>();
        float distance = Vector3.Distance(enemyFSM.transform.position, enemyFSM.Player.transform.position);
        return distance < 10f;

    }

    public static bool PlayerNotClose(IStateMachine fsm)
    {
        MonoStateMachine monoStateMachine = fsm as MonoStateMachine;
        EnemyFSM enemyFSM = monoStateMachine.GetComponent<EnemyFSM>();
        float distance = Vector3.Distance(enemyFSM.transform.position, enemyFSM.Player.transform.position);
        return !(distance < 10f);

    }

    public static void ChasePlayer(IStateMachine fsm)
    {
        MonoStateMachine monoStateMachine = fsm as MonoStateMachine;
        EnemyFSM enemyFSM = monoStateMachine.GetComponent<EnemyFSM>();
        Vector3 target = enemyFSM.Player.transform.position;
        Vector3 directionVector = (target - enemyFSM.transform.position).normalized;
        enemyFSM.GetComponent<Rigidbody>().velocity = directionVector * enemyFSM.Speed;

    }


}
