using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSpriteManager : MonoBehaviour
{
    public GameObject bike;
    public GameObject[] right1 = new GameObject[3];
    public GameObject[] right2 = new GameObject[3];
    public GameObject[] up = new GameObject[3];
    public GameObject[] down = new GameObject[3];

    private int player;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        player = 0;
        rb = bike.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        up[player].SetActive(Input.GetKey(KeyCode.W));
        down[player].SetActive(Input.GetKey(KeyCode.S));

        right1[player].SetActive(!up[player].activeSelf || !down[player].activeSelf);
    }
}
