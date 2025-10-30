using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerBike : MonoBehaviour
{
    //public variables
    public float speed = 0f;
    public float maxSpeed = 22f;
    public float acc = 25f;
    public float strafeAcc = 25f;
    public float temp;
    public float maxTempSpeed = 30;
    public float tempAcc = 35;
    public float tempIncreaseRate = 20f;
    public float tempDecreaseRate = 10f;
    public float maxTemp = 1000f;
    public float timer;
    public bool timing;
    public bool recharge;
    public bool endGame;

    //private variables
    private float topAcc;
    private Rigidbody rb;
    private bool onGround;
    private bool flip;
    private int lap;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 0;
        recharge = false;
        lap = 0;
        timer = 0;
        timing = true;
        endGame = false;
        maxTemp = 1000;
        strafeAcc = 25;

        if (CharacterManager.player == 0)
        {
            //default settings
            maxSpeed = 22;
            acc = 25;
            maxTempSpeed = 30;
            tempAcc = 35;
            tempIncreaseRate = 7;
            tempDecreaseRate = 3;
        }
        else if(CharacterManager.player == 1)
        {
            //focus temp
            maxSpeed = 21;
            acc = 24;
            maxTempSpeed = 27;
            tempAcc = 34;
            tempIncreaseRate = 4.5f;
            tempDecreaseRate = 3.3f;
        }
        else
        {
            //focus speed
            maxSpeed = 25;
            acc = 26.5f;
            maxTempSpeed = 40;
            tempAcc = 36;
            tempIncreaseRate = 10;
            tempDecreaseRate = 2.3f;
        }
        topAcc = acc;
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
            acc = topAcc + 10;
        }
        else
        {
            acc = topAcc;
        }

        //accelerate regularly
        if (Input.GetKey(KeyCode.Period) && speed < maxSpeed && onGround)
        {
            rb.AddForce(Vector3.right * acc, ForceMode.Acceleration);
            //keep it within the max speed while accelerating
            if (speed >= maxSpeed)
            {
                //lower the max speed when the temp gauge is recharging
                if (recharge)
                {
                    rb.velocity = new Vector3(maxSpeed - 5, rb.velocity.y);
                }
                rb.velocity = new Vector3(maxSpeed, rb.velocity.y);
            }
        }
       
        //accelerate via temp
        if (Input.GetKey(KeyCode.Comma) && speed < maxTempSpeed && onGround && !recharge)
        {
            //increase the temp gauge when using the special acceleration
            temp += tempIncreaseRate;
            rb.AddForce(Vector3.right * tempAcc, ForceMode.Acceleration);
            //keep it within the max speed while accelerating
            if (speed >= maxTempSpeed)
            {
                rb.velocity = new Vector3(maxTempSpeed, 0);
            }
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

        //if the player has maxed out the limit, start recharging and penalize them
        if(temp >= maxTemp)
        {
            temp = maxTemp;
            recharge = true;
        }

        //decrease the gauge
        if (!Input.GetKey(KeyCode.Comma) && Time.timeScale != 0)
        {
            temp -= tempDecreaseRate;
        }
        //keep temp from becoming negative
        if (temp < 0)
        {
            temp = 0;
        }

        //The temp gauge has reached 0, so it is no longer recharging
        if (temp == 0)
        {
            recharge = false;
        }

        if (timing)
        {
            timer += Time.deltaTime;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        //manages the laps
        if (other.CompareTag("Portal"))
        {
            lap++;
            if (lap < 2)
            {
                transform.position = new Vector3(-425.2f, transform.position.y, transform.position.z);
            }
            else
            {
                timing = false;
            }
        }

        //slows down when it goes in mud or grass
        if (other.CompareTag("Mud"))
        {
            rb.velocity = new Vector3(rb.velocity.x - 20, rb.velocity.y, rb.velocity.z);
        }

        //resets the temp gauge when it goes through an arrow
        if (other.CompareTag("Arrow"))
        {
            temp = 0;
        }

        //gives it "friction" on the ground, but prevents it from massively slowing down
        //in the air
        if (other.CompareTag("TrackPart"))
        {
            rb.drag = 0.7f;
        }
        else
        {
            rb.drag = 0.45f;
        }

        //when the game is over
        if (other.CompareTag("EndZone"))
        {
            //end game and go to the in between menu
            endGame = true;
        }
    }

    public bool Grounded()
    {
        if (Physics.Raycast(transform.position, -transform.up, (transform.localScale.x / 2) + 0.07f))
        {
            return true;
        }
        return false;
    }
    private bool Flipped()
    {
        if (Physics.Raycast(transform.position, Vector3.down, (transform.localScale.x / 2) + 0.07f) && transform.eulerAngles.z != 0)
        {
            return true;
        }
        return false;
    }
}
