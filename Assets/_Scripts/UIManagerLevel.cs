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
    public AudioSource levelBGM;
    public AudioSource winBGM;
    public TextMeshProUGUI countdown;
    public GameObject bike;
    public AudioSource tickSound;
    public float winTime;

    private PlayerBike bikeScript;
    private RectTransform tempTransform;
    private int countdownTime;

    // Start is called before the first frame update
    void Start()
    {
        bikeScript = bike.GetComponent<PlayerBike>();
        tempTransform = tempGauge.GetComponent<RectTransform>();
        tempTransform.localScale = new Vector3(0, tempTransform.localScale.y);
        endMessage.enabled = false;
        timerMessage.enabled = false;
        winBGM.mute = true;
        bikeScript.enabled = false;

        //script provided by ChatGPT
        StartCoroutine(CountdownCoroutine());
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
            levelBGM.Pause();
            winBGM.mute = false;

            endMessage.enabled = true;
            timerMessage.enabled = true;
            //the player wins
            if (bikeScript.timer <= winTime)
            {
                winImage.SetActive(true);
                loseImage.SetActive(false);
                nextButton.SetActive(true);
                menuButton.SetActive(false);
                endMessage.text = "YOU WIN!";
            }
            //the player loses
            else
            {
                loseImage.SetActive(true);
                winImage.SetActive(false);
                menuButton.SetActive(true);
                nextButton.SetActive(false);
                endMessage.text = "YOU LOSE...";
            }

            timerMessage.text = $"TIME: {(int)bikeScript.timer / 60}:{(bikeScript.timer % 60).ToString("00.00")}";
            endScreen.SetActive(true);
        }
    }

    //script provided by ChatGPT
    private System.Collections.IEnumerator CountdownCoroutine()
    {
        countdownTime = 3;
        while (countdownTime > 0)
        {
            tickSound.Play();
            countdown.text = countdownTime.ToString();
            yield return new WaitForSeconds(0.6f);
            countdownTime--;
        }
        yield return new WaitForSeconds(0.61f);
        tickSound.Play();
        countdown.text = "GO!";

        //start the scene now that the countdown has concluded
        bikeScript.enabled = true;
        levelBGM.Play();

        yield return new WaitForSeconds(1f);
        countdown.text = "";
    }
}
