using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public GameObject pauseScreen;

    public void ContinueGame()
    {
        pauseScreen.SetActive(false);
        Time.timeScale = 1;
    }
}
