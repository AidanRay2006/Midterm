using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject follow;
    public float min = -491;
    public float max = 491;

    private Vector3 followPos;
    // Start is called before the first frame update
    void Start()
    {
        //starts the camera at the current positon of the object its following
        //but only its x
        followPos = new Vector3(follow.transform.position.x, transform.position.y, transform.position.z);
        transform.position = followPos;
    }

    // Update is called once per frame
    void Update()
    {
        //gets the position of the object it's following every frame
        Vector3 followPos = new Vector3(follow.transform.position.x, transform.position.y, transform.position.z);

        //the camera only follows the object if it is within certain bounds
        if (transform.position.x < max)
        {
            transform.position = followPos;
        }
    }
}
