//included to try and lower GPU usage
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    public int target = 30;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //script from Brogan89 on Unity Discussions
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = target;
    }
    void Update()
    {
        //script from Brogan89 on Unity Discussions
        if (Application.targetFrameRate != target)
        {
            Application.targetFrameRate = target;
        }
    }
}
