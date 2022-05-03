using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : FiniteStateMachine
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

    private void Awake()
    {

        idle = new EnemyIdleState();
        chase = new EnemyChaseState();
        CurrentState = idle;

    }



}
