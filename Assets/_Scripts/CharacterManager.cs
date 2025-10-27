using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static int player;

    //provided by Gemini
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
