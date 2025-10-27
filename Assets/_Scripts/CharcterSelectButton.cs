using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharcterSelectButton : MonoBehaviour
{
    public static void SelectCharacter(int character)
    {
        CharacterManager.player = character;
        SceneManager.LoadScene("Level1");
    }
}
