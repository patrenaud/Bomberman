using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombBehavior : MonoBehaviour
{
    public GameObject m_Blast;
    
    private int m_Row;
    private int m_Col;
    private Vector2 BombPos;
    [SerializeField] private GameObject m_PowerBomb;

    public void Start()
    {
        StartCoroutine(BombSetup());
    }

    public void Setup(int a_CurrentCol, int a_CurrentRow)
    {
        m_Row = a_CurrentRow;
        m_Col = a_CurrentCol;
        BombPos = new Vector2(m_Row, m_Col);
    }

    private IEnumerator BombSetup()
    {
        yield return new WaitForSeconds(3.8f);
        StartCoroutine(BombBlast());
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private IEnumerator BombBlast()
    {
        List<Vector2> AllBombsPos = new List<Vector2>();
        List<Vector2Int> AllBlastIndex = new List<Vector2Int>();

        for (int i = 0; i <= LevelGenerator.Instance.m_PlayerPrefab.m_BombRadius; i++)
        {
            if (i != 0)
            {
                // Rempli la liste de position des blasts
                AllBombsPos.Add(LevelGenerator.Instance.GetPositionAt((int)BombPos.y, (int)BombPos.x + i));
                AllBombsPos.Add(LevelGenerator.Instance.GetPositionAt((int)BombPos.y, (int)BombPos.x - i));

                // Rempli la liste d'index des blasts
                AllBlastIndex.Add(new Vector2Int(m_Row, m_Col + i));
                AllBlastIndex.Add(new Vector2Int(m_Row, m_Col - i));

                for (int j = 0; j <= LevelGenerator.Instance.m_PlayerPrefab.m_BombRadius; j++)
                {
                    if (j != 0)
                    {
                        // Rempli la liste de position des blasts
                        AllBombsPos.Add(LevelGenerator.Instance.GetPositionAt((int)BombPos.y + j, (int)BombPos.x));
                        AllBombsPos.Add(LevelGenerator.Instance.GetPositionAt((int)BombPos.y - j, (int)BombPos.x));
                        // Rempli la liste d'index des blasts
                        AllBlastIndex.Add(new Vector2Int(m_Row + j, m_Col));
                        AllBlastIndex.Add(new Vector2Int(m_Row - j, m_Col));
                    }
                }
            }
        }

        for (int i = 0; i < AllBlastIndex.Count; i++)
        {
            // On crée les Blasts de bombes aux index de la liste
            GameObject blast = Instantiate(m_Blast, AllBombsPos[i], Quaternion.identity);
            blast.GetComponent<AutoDestroy>().Setup(AllBlastIndex[i].x, AllBlastIndex[i].y);

            if (LevelGenerator.Instance.GetTileTypeAtIndex(AllBlastIndex[i].y, AllBlastIndex[i].x) == ETileType.Destructable)
            {
                LevelGenerator.Instance.SetTileTypeAtIndex(AllBlastIndex[i].y, AllBlastIndex[i].x);
            }
        }
        yield return new WaitForSeconds(1f);
    }
}
