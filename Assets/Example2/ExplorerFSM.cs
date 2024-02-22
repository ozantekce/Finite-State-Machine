using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorerFSM : MonoStateMachine
{

    [SerializeField]
    private float speed;


    private ConcreteState moveForwardState, moveRightState, moveLeftState, idleState;

    private void Awake()
    {

        

        moveForwardState = new ConcreteState();
        moveRightState = new ConcreteState();
        moveLeftState = new ConcreteState();
        idleState = new ConcreteState();

        moveForwardState.AddAction(new MyAction(MoveForward));
        moveForwardState.AddTransition(new Transition(moveRightState, Obstacle));



        moveRightState.AddAction(new MyAction(MoveRight));
        moveRightState.AddTransition(new Transition(moveForwardState, NoObstacle));
        moveRightState.AddTransition(new Transition(moveLeftState, ReachedTheRightBorder));



        
        moveLeftState.AddAction(new MyAction(MoveLeft));
        moveLeftState.AddTransition(new Transition(moveForwardState, NoObstacle));
        moveLeftState.AddTransition(new Transition(idleState, ReachedTheLeftBorder));

        

        
        idleState.AddAction(new MyAction((fsm) =>
        {

            Debug.Log("OVER");

        }
        ),Runtime.Enter);


        ChangeCurrentState(moveForwardState);

    }



    private bool NoObstacle(IStateMachine fsm)
    {

        RaycastHit hit;

        if(Physics.Raycast(transform.position, Vector3.forward,out hit, 2f))
        {
            if(hit.transform.tag == "Obstacle")
            {
                return false;
            }

        }

        return true;

    }

    private bool Obstacle(IStateMachine fsm)
    {

        return !NoObstacle(fsm);

    }



    private bool ReachedTheRightBorder(IStateMachine fsm)
    {


        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.right, out hit, 2f))
        {
            if (hit.transform.tag == "Border")
            {
                return true;
            }

        }

        return false;


    }

    private bool ReachedTheLeftBorder(IStateMachine fsm)
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.left, out hit, 2f))
        {
            if (hit.transform.tag == "Border")
            {
                return true;
            }

        }

        return false;


    }

    private void MoveForward(IStateMachine fsm)
    {
        transform.Translate(Vector3.forward*speed);
    }


    private void MoveLeft(IStateMachine fsm)
    {
        transform.Translate(Vector3.left * speed);

    }

    private void MoveRight(IStateMachine fsm)
    {
        transform.Translate(Vector3.right * speed);

    }




}
