using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPowerUp1 : MonoBehaviour
{
    public int m_CurrentCol;
    public int m_CurrentRow;

    private bool m_CanUpgrade = true;

    public void Setup(int aRow, int aCol)
    {
        LevelGenerator.Instance.m_BombPowerup1 = this;
        m_CurrentRow = aRow;
        m_CurrentCol = aCol;
    }

    private void Update()
    {
        if (m_CurrentRow == LevelGenerator.Instance.m_PlayerPrefab.m_CurrentRow && m_CurrentCol == LevelGenerator.Instance.m_PlayerPrefab.m_CurrentCol && m_CanUpgrade)
        {
            m_CanUpgrade = false;
            LevelGenerator.Instance.m_PlayerPrefab.m_BombRadius += 1;
            StartCoroutine(PowerUpDelay());
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private IEnumerator PowerUpDelay()
    {
        // Le PowerUp du BombRadius ne va durer seulement que 8 secondes
        yield return new WaitForSeconds(10f);
        LevelGenerator.Instance.m_PlayerPrefab.m_BombRadius -= 1;
        Destroy(gameObject);
    }
}


