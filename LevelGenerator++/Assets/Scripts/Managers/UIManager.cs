using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : DontDestroyOnLoad
{
    [SerializeField] private Slider m_HealthBar;
    [SerializeField] private Image m_BackGround;
    [SerializeField] private Button m_StartButton;

    private static UIManager m_Instance;
    public static UIManager Instance
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
        m_HealthBar.interactable = false;
        m_HealthBar.gameObject.SetActive(false);
        m_HealthBar.value = 1;
    }

    public void AjustHealth()
    {
        m_HealthBar.value -= 0.2f;
    }

    public void SwitchMainToGamePlay()
    {
        m_HealthBar.gameObject.SetActive(true);
        m_BackGround.gameObject.SetActive(false);
        m_StartButton.gameObject.SetActive(false);
    }

    public void ResetHealth()
    {
        m_HealthBar.value = 1;
    }
}
