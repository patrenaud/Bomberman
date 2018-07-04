using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudMenu : MonoBehaviour
{
    public GameObject m_BackGround;

    public void SwitchToGamePlay()
    {
        SceneManagement.Instance.ChangeLevel("GamePlay");
        UIManager.Instance.ResetHealth();
        UIManager.Instance.SwitchMainToGamePlay();
        gameObject.SetActive(false);
    }

    public void SwitchToMainMenu()
    {
        SceneManagement.Instance.ChangeLevel("ApplicationLauncher");
        UIManager.Instance.ResetHealth();
    }

    public void SwitchToResults()
    {
        SceneManagement.Instance.ChangeLevel("Results");
        UIManager.Instance.ResetHealth();
    }

    public void CloseBackGround()
    {
        m_BackGround.SetActive(false);
    }

}
