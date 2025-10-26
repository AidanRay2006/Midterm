using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI timerText;
    public GameObject tempGauge;
    public GameObject endScreen;
    public GameObject nextButton;
    public GameObject menuButton;
    public GameObject winImage;
    public GameObject loseImage;
    public TextMeshProUGUI endMessage;
    public TextMeshProUGUI timerMessage;
    public GameObject pauseMenu;

    private PlayerBike bikeScript;
    private RectTransform tempTransform;

    // Start is called before the first frame update
    void Start()
    {
        bikeScript = FindObjectOfType<PlayerBike>();
        tempTransform = tempGauge.GetComponent<RectTransform>();
        tempTransform.localScale = new Vector3(0, tempTransform.localScale.y);
        endMessage.enabled = false;
        timerMessage.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //update the temp gauge on the UI
        tempTransform.localScale = new Vector3(bikeScript.temp/10000, tempTransform.localScale.y);
        if (bikeScript.recharge)
        {
            tempText.color = Color.red;
        }
        else
        {
            tempText.color = Color.white;
        }
        //formatting code thanks to Gemini
        timerText.text = $"{(int)bikeScript.timer / 60}:{(bikeScript.timer % 60).ToString("00.00")}";

        //bring up the pause menu
        if (Input.GetKeyDown(KeyCode.Escape) && !bikeScript.endGame)
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
            if (pauseMenu.activeSelf)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

        //end game UI
        if (bikeScript.endGame)
        {
            endMessage.enabled = true;
            timerMessage.enabled = true;
            //the player wins
            if (bikeScript.timer <= 120)
            {
                winImage.SetActive(true);
                nextButton.SetActive(true);
                menuButton.SetActive(false);
                endMessage.text = "YOU WIN!";
            }
            //the player loses
            else
            {
                loseImage.SetActive(true);
                menuButton.SetActive(true);
                nextButton.SetActive(false);
                endMessage.text = "YOU LOSE...";
            }

            timerMessage.text = $"TIME: {(int)bikeScript.timer / 60}:{(bikeScript.timer % 60).ToString("00.00")}";
            endScreen.SetActive(true);
        }
    }
}
