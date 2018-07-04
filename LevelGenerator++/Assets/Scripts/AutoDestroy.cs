using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private int m_Row;
    private int m_Col;
    private bool m_Damage = true;

    private void Start()
    {
        StartCoroutine(AutoDestroyer());
    }
    public void Setup(int a_CurrentCol, int a_CurrentRow)
    {
        m_Row = a_CurrentRow;
        m_Col = a_CurrentCol;
    }

    private void Update()
    {
        if (m_Row == LevelGenerator.Instance.m_PlayerPrefab.m_CurrentRow && m_Col == LevelGenerator.Instance.m_PlayerPrefab.m_CurrentCol && m_Damage)
        {
            m_Damage = false;
            LevelGenerator.Instance.m_PlayerPrefab.DamagePlayer();
            Destroy(gameObject);
            m_Damage = true;
        }

        if (m_Row == LevelGenerator.Instance.m_EnemyPrefab.m_CurrentRow && m_Col == LevelGenerator.Instance.m_EnemyPrefab.m_CurrentCol && m_Damage)
        {
            Destroy(gameObject);
            Destroy(LevelGenerator.Instance.m_EnemyPrefab.gameObject);
            SceneManagement.Instance.ChangeLevel("Results");
        }
    }

    private IEnumerator AutoDestroyer()
    {
        yield return new WaitForSeconds(0.8f);
        Destroy(gameObject);
    }
}