using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBike : MonoBehaviour
{
    //public variables
    public float speed = 0f;
    public float maxSpeed = 55f;
    public float acc = 2f;
    public float strafeAcc = 2f;

    //private variables
    private Rigidbody rb;
    private bool onGround;
    private bool flip;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        speed = rb.velocity.x;
        onGround = Grounded();
        flip = Flipped();

        //get the bike upright
        if (flip)
        {
            transform.eulerAngles = Vector3.zero;
        }

        //makes it easier to go faster
        if(speed < 16.5f)
        {
            acc = 35f;
        }
        else
        {
            acc = 25f;
        }

        //accelerate regularly
        if (Input.GetKey(KeyCode.Period) && speed < maxSpeed && onGround)
        {
            rb.AddForce(Vector3.right * acc, ForceMode.Acceleration);
        }
        else if (speed >= maxSpeed)
        {
            rb.velocity = new Vector3(maxSpeed, 0);
        }

        //move up and down
        if (speed > 0 && Input.GetKey(KeyCode.W) && onGround)
        {
            rb.AddForce(Vector3.forward * strafeAcc, ForceMode.Force);
        }
        if (speed > 0 && Input.GetKey(KeyCode.S) && onGround)
        {
            rb.AddForce(Vector3.back * strafeAcc, ForceMode.Force);
        }

    }

    public bool Grounded()
    {
        if (Physics.Raycast(transform.position, -transform.up, transform.localScale.x / 2))
        {
            return true;
        }
        return false;
    }
    private bool Flipped()
    {
        if (Physics.Raycast(transform.position, Vector3.down, transform.localScale.x / 2) && transform.eulerAngles.z != 0)
        {
            return true;
        }
        return false;
    }
}
