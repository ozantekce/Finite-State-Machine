using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{

    [SerializeField]
    private float m_Speed;
    [SerializeField]
    private float acceleration;


    private Rigidbody rigidbody;

    private bool moveForwardInput;
    private bool moveBackwardInput;
    private bool spinRightInput;
    private bool spinLeftInput;



    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void Update()
    {
        InputReader();
    }

    private void FixedUpdate()
    {

        Movement();

    }



    private void InputReader()
    {

        moveForwardInput = Input.GetAxis("Vertical") > 0;
        moveBackwardInput = Input.GetAxis("Vertical") < 0;
        spinRightInput = Input.GetAxis("Horizontal") > 0;
        spinLeftInput = Input.GetAxis("Horizontal") < 0;


    }

    private void Movement()
    {

        Vector3 force = Vector3.zero;

        if (moveForwardInput)
        {
            force += Vector3.forward;
        }
        if (moveBackwardInput)
        {
            force -= Vector3.forward;
        }
        if (spinRightInput)
        {
            transform.Rotate(Vector3.up);
        }
        if (spinLeftInput)
        {
            transform.Rotate(Vector3.down);
        }

        force = (force.normalized * acceleration);

        float newVelocity = acceleration + rigidbody.velocity.magnitude;


        if(newVelocity <= m_Speed)
        {
            rigidbody.AddRelativeForce(force, ForceMode.VelocityChange);
        }



    }



}
