using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehavior : MonoBehaviour
{
    private float m_offset = 64f / 100f; // = Pixel size / Pixel per Unit

    [SerializeField]
    public GameObject m_Blast;
    public int m_BombRadius = 1;
    private LevelGenerator m_Level;

    private void Start()
    {
        StartCoroutine(BombSetup());
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
        Vector2 BombPos = new Vector2(transform.position.x, transform.position.y);

        List<Vector2> AllBombsPos = new List<Vector2>();

        for (int i = 0; i <= m_BombRadius; i++)
        {
            if (i != 0)
            {
                // Instanciate bomb in horizontal radius
                AllBombsPos.Add(new Vector2(BombPos.x + (i * m_offset), BombPos.y));
                AllBombsPos.Add(new Vector2(BombPos.x - (i * m_offset), BombPos.y));
                for (int j = 0; j <= m_BombRadius; j++)
                {
                    // Instanciate bomb in vertival radius
                    AllBombsPos.Add(new Vector2(BombPos.x, BombPos.y + (i * m_offset)));
                    AllBombsPos.Add(new Vector2(BombPos.x, BombPos.y - (i * m_offset)));
                }
            }
        }

        for (int i = 0; i < AllBombsPos.Count; i++)
        {
            //if(m_Level.GetTileTypeAtPos(AllBombsPos[i].x, AllBombsPos[i].y) == "Wall )

            GameObject Instance = Instantiate(m_Blast, AllBombsPos[i], Quaternion.identity);
        }
        yield return new WaitForSeconds(1f);
    }
}
