using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : DontDestroyOnLoad
{

    public void LoadGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
