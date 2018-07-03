using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private int m_Row;
    private int m_Col;

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
        if (m_Row == LevelGenerator.Instance.m_PlayerPrefab.m_CurrentRow && m_Col == LevelGenerator.Instance.m_PlayerPrefab.m_CurrentCol)
        {            
            LevelGenerator.Instance.m_PlayerPrefab.m_CurrentHp -= 1;
        }
    }

    private IEnumerator AutoDestroyer()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

}