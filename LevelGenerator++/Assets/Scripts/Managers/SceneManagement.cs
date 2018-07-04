using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : DontDestroyOnLoad
{
    [SerializeField] private GameObject m_LoadingScreen;

    private static SceneManagement m_Instance;
    public static SceneManagement Instance
    {
        get { return m_Instance; }
    }

    protected override void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
        }
        else
        {
            Destroy(this);
        }
        base.Awake();
    }

    private void StartLoading()
    {
        m_LoadingScreen.SetActive(true);
    }

    private void OnLoadingDone(Scene i_Scene, LoadSceneMode i_Mode)
    {
        SceneManager.sceneLoaded -= OnLoadingDone;
        m_LoadingScreen.SetActive(false);
        //m_IsLoadingDone = true;
    }

    public void ChangeLevel(string i_Scene)
    {
        StartLoading();
        
        StartCoroutine(LoadingTimer(i_Scene));

        SceneManager.sceneLoaded += OnLoadingDone;
    }

    private IEnumerator LoadingTimer(string i_Scene)
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(i_Scene);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GamePlay");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("ApplicationLauncher");
    }

    public void LoadResults()
    {
        SceneManager.LoadScene("Results");
    }

    public void LoadDeath()
    {
        SceneManager.LoadScene("Death");
    }
}
