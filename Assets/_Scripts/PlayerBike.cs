using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI timerText;

    //private variables
    private Rigidbody rb;
    private bool onGround;
    private bool flip;
    private bool recharge;
    private int lap;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 0;
        recharge = false;
        lap = 0;
        timer = 0;
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
        if (!Input.GetKey(KeyCode.Comma))
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

        timer += Time.deltaTime;

        //update the temp gauge on the UI
        tempText.text = $"Temp: {temp}/{maxTemp}";
        //formatting code thanks to Gemini
        timerText.text = $"{(int)timer / 60}:{string.Format("{0:F2}", timer%60)}";
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            lap++;
            if (lap < 2)
            {
                transform.position = new Vector3(-425.2f, transform.position.y, transform.position.z);
            }
        }
        if (other.CompareTag("Mud"))
        {
            rb.velocity = new Vector3(rb.velocity.x - 20, rb.velocity.y, rb.velocity.z);
        }
        if (other.CompareTag("Arrow"))
        {
            temp = 0;
        }
    }

    public bool Grounded()
    {
        if (Physics.Raycast(transform.position, -transform.up, (transform.localScale.x / 2) + 0.02f))
        {
            return true;
        }
        return false;
    }
    private bool Flipped()
    {
        if (Physics.Raycast(transform.position, Vector3.down, (transform.localScale.x / 2) + 0.02f) && transform.eulerAngles.z != 0)
        {
            return true;
        }
        return false;
    }
}
