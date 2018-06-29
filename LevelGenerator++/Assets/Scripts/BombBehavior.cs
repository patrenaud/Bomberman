using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    public GameObject m_Blast;
    public int m_BombRadius = 1;

    private int m_Row;
    private int m_Col;
    private Vector2 BombPos;


    private void Start()
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
        StartCoroutine(BombRadius());
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private IEnumerator BombRadius()
    {
        Debug.Log("BombPosition: " + BombPos);
        List<Vector2> AllBombsPos = new List<Vector2>();
        List<Vector2> AllBlastIndex = new List<Vector2>();

        for (int i = 0; i <= m_BombRadius; i++)
        {
            if (i != 0)
            {
                // Instanciate blast in horizontal radius
                AllBombsPos.Add(LevelGenerator.Instance.GetPositionAt((int)BombPos.y, (int)BombPos.x + i));
                AllBombsPos.Add(LevelGenerator.Instance.GetPositionAt((int)BombPos.y, (int)BombPos.x - i));

                AllBlastIndex.Add(new Vector2(m_Row, m_Col + i));
                AllBlastIndex.Add(new Vector2(m_Row, m_Col - i));


                for (int j = 0; j <= m_BombRadius; j++)
                {
                    if (j != 0)
                    {
                        // Instanciate blast in vertival radius
                        AllBombsPos.Add(LevelGenerator.Instance.GetPositionAt((int)BombPos.y + j, (int)BombPos.x));
                        AllBombsPos.Add(LevelGenerator.Instance.GetPositionAt((int)BombPos.y - j, (int)BombPos.x));

                        AllBlastIndex.Add(new Vector2(m_Row + j, m_Col));
                        AllBlastIndex.Add(new Vector2(m_Row - j, m_Col));
                    }
                }
            }
        }

        for (int i = 0; i < AllBlastIndex.Count; i++)
        {
            Instantiate(m_Blast, AllBombsPos[i], Quaternion.identity);

            // En utilisant le debug, j'ai du inverser le signe du x puisqu'il était négetif
            Debug.Log("Blast Position: " + AllBombsPos[i]);
            if (LevelGenerator.Instance.GetTileTypeAtIndex((int)AllBlastIndex[i].y, (int)AllBlastIndex[i].x) == ETileType.Destructable)
            {
                Debug.Log("SetTileType");
                LevelGenerator.Instance.SetTileTypeAtIndex((int)AllBlastIndex[i].y, (int)AllBlastIndex[i].x);
            }
        }
        yield return new WaitForSeconds(1f);
    }
}
