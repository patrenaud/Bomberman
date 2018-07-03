using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private static LevelGenerator m_Instance;
    public static LevelGenerator Instance
    {
        get { return m_Instance; }
    }

    private const float PIXEL_PER_UNIT = 100;
    private const float TILE_SIZE = 64;
    private List<List<GameObject>> m_TileObjects = new List<List<GameObject>>();    

    public GameObject[] m_FloorPrefabList;
    public GameObject[] m_WallPrefabList;
    public GameObject[] m_DestructiblePrefabList;

    public LevelData m_LevelData;

    public PlayerMovement m_PlayerPrefab;
    public EnemyMovement m_EnemyPrefab;

    public BombPowerUp1 m_Powerup1;

    private void Awake()
    {
        m_Instance = this;

        float x = (-Screen.width + TILE_SIZE) / PIXEL_PER_UNIT / 2.0f;
        float y = (Screen.height - TILE_SIZE) / PIXEL_PER_UNIT / 2.0f;
        Vector2 initialPos = new Vector2(x, y);

        Vector2 offset = new Vector2(TILE_SIZE / PIXEL_PER_UNIT, -TILE_SIZE / PIXEL_PER_UNIT);
        Vector2 spawnPos = initialPos + offset;
        Vector2 EnemySpawnPos = initialPos + offset + new Vector2(8, 8);
        Vector2 Power1Pos = initialPos + offset + new Vector2(2, 2);

        PlayerMovement player = Instantiate(m_PlayerPrefab, spawnPos, Quaternion.identity);
        player.Setup(1, 1);
        EnemyMovement enemy = Instantiate(m_EnemyPrefab, EnemySpawnPos, Quaternion.identity);
        enemy.Setup(9, 9);
        BombPowerUp1 Powerup1 = Instantiate(m_Powerup1, Power1Pos, Quaternion.identity);


        for (int i = 0; i < m_LevelData.GetWidth(); ++i)
        {
            m_TileObjects.Add(new List<GameObject>());
            for (int j = 0; j < m_LevelData.GetHeight(); ++j)
            {
                offset = new Vector2(TILE_SIZE * i / PIXEL_PER_UNIT, -TILE_SIZE * j / PIXEL_PER_UNIT);
                spawnPos = initialPos + offset;
                
                m_TileObjects[i].Add(CreateTile(m_LevelData.Tiles[i][j], spawnPos));
            }
        }

        for (int i = 0; i < m_LevelData.Tiles.Length; i++)
        {
            m_LevelData.Tiles[i].SetCopy();
        }
    }

    private GameObject CreateTile(ETileType aType, Vector2 aPos)
    {
        // ON DOIT AJOUTER UN RANDOM POUR CHOISIR UNE IMAGE DANS LA LISTE
        switch (aType)
        {
            case ETileType.Floor:
                GameObject floor = Instantiate(m_FloorPrefabList[Random.Range(0, m_FloorPrefabList.Length)]);
                floor.transform.position = aPos;
                return floor;
            case ETileType.Wall:
                GameObject wall = Instantiate(m_WallPrefabList[Random.Range(0, m_WallPrefabList.Length)]);
                wall.transform.position = aPos;
                return wall;
            case ETileType.Destructable:
                GameObject Destructable = Instantiate(m_DestructiblePrefabList[Random.Range(0, m_WallPrefabList.Length)]);
                Destructable.transform.position = aPos;
                return Destructable;
        }
        return null;
    }

    // Ici on voit le type de la tuile
    public ETileType GetTileTypeAtIndex(int aRow, int aCol)
    {
        if (aRow >= 0 && aRow < m_LevelData.GetWidth())
        {
            if (aCol >= 0 && aCol < m_LevelData.GetHeight())
            {
                return (ETileType)m_LevelData.Tiles[aCol][aRow];
            }
        }
        return ETileType.Wall;
    }

    // Set tile type 
    public void SetTileTypeAtIndex(int aRow, int aCol)
    {
        Vector2 pos;
        m_LevelData.Tiles[aCol][aRow] = ETileType.Floor;
        pos = m_TileObjects[aCol][aRow].transform.position;
        Destroy(m_TileObjects[aCol][aRow].gameObject);
        m_TileObjects[aCol][aRow] = Instantiate(m_FloorPrefabList[Random.Range(0, m_FloorPrefabList.Length)], pos, Quaternion.identity);
    }


    public Vector3 GetPositionAt(int aRow, int aCol)
    {
        float x = (-Screen.width + TILE_SIZE) / PIXEL_PER_UNIT / 2.0f;
        float y = (Screen.height - TILE_SIZE) / PIXEL_PER_UNIT / 2.0f;
        Vector2 initialPos = new Vector2(x, y);

        Vector2 offset = new Vector2(TILE_SIZE * aCol / PIXEL_PER_UNIT, -TILE_SIZE * aRow / PIXEL_PER_UNIT);
        Vector2 pos = initialPos + offset;

        return pos;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < m_LevelData.Tiles.Length; i++)
        {
            m_LevelData.Tiles[i].ResetCopy();
        }
    }
}
