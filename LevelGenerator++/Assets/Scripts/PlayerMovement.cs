using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public int m_CurrentRow;
    public int m_CurrentCol;
    public int m_BombRadius;
    public float m_CurrentSpeed;
    public float m_CurrentHp;

    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject m_BombPrefab;
    [SerializeField] private PlayerData m_Data;
    private List<GameObject> m_Bombs = new List<GameObject>();
    private bool m_IsMoving = false;
    private bool m_CanPlaceBomb = true;
    private Vector2 m_InitialPos;
    private Vector2 m_WantedPos;
    private float m_PercentageCompletion;    
    private float m_InputX;
    private float m_InputY;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_CurrentHp = m_Data.HP;
        m_CurrentSpeed = m_Data.Speed;
        m_BombRadius = m_Data.BombRadius;
    }

    public void Setup(int aRow, int aCol)
    {
        LevelGenerator.Instance.m_PlayerPrefab = this;
        m_CurrentRow = aRow;
        m_CurrentCol = aCol;
    }

    // Update is called once per frame
    private void Update()
    {
        if (!m_IsMoving)
        {
            float askMoveHorizontal = Input.GetAxisRaw("Horizontal");
            float askMoveVertical = Input.GetAxisRaw("Vertical");

            if (askMoveHorizontal != 0 &&
                LevelGenerator.Instance.GetTileTypeAtIndex(m_CurrentRow, m_CurrentCol + (int)askMoveHorizontal) == ETileType.Floor ||
                LevelGenerator.Instance.GetTileTypeAtIndex(m_CurrentRow, m_CurrentCol + (int)askMoveHorizontal) == ETileType.Trap)
            {
                m_IsMoving = true;
                m_PercentageCompletion = 0f;

                m_InitialPos = transform.position;
                m_WantedPos = LevelGenerator.Instance.GetPositionAt(m_CurrentRow, m_CurrentCol + (int)askMoveHorizontal);

                m_CurrentCol += (int)askMoveHorizontal;
            }
            else if (askMoveVertical != 0 &&
                LevelGenerator.Instance.GetTileTypeAtIndex(m_CurrentRow - (int)askMoveVertical, m_CurrentCol) == ETileType.Floor ||
                LevelGenerator.Instance.GetTileTypeAtIndex(m_CurrentRow - (int)askMoveVertical, m_CurrentCol) == ETileType.Trap)
            {
                m_IsMoving = true;
                m_PercentageCompletion = 0f;

                m_InitialPos = transform.position;
                m_WantedPos = LevelGenerator.Instance.GetPositionAt(m_CurrentRow - (int)askMoveVertical, m_CurrentCol);
                m_CurrentRow -= (int)askMoveVertical;
            }
        }

        // Ceci contrôle les animations du Player
        m_InputX = Input.GetAxisRaw("Horizontal");
        m_InputY = Input.GetAxisRaw("Vertical");
        m_Animator.SetInteger("MoveHorizontal", (int)m_InputX);
        m_Animator.SetInteger("MoveVertical", (int)m_InputY);

        // Pour placer la bombe
        if (Input.GetKeyDown(KeyCode.Space) && m_CanPlaceBomb)
        {
            m_CanPlaceBomb = false;
            StartCoroutine(BombDelay());
            m_Bombs.Add(m_BombPrefab);
            GameObject instance = Instantiate(m_BombPrefab, transform.position, Quaternion.identity);
            instance.GetComponent<BombBehavior>().Setup(m_CurrentRow, m_CurrentCol);
        }

        // Ceci est pour le piège
        if (LevelGenerator.Instance.GetTileTypeAtIndex(m_CurrentRow, m_CurrentCol) == ETileType.Trap)
        {
            LevelGenerator.Instance.SetTileTypeAtIndex(m_CurrentRow, m_CurrentCol);
            DamagePlayer();
        }
    }

    public void DamagePlayer()
    {
        m_CurrentHp--;
        UIManager.Instance.AjustHealth();
        if(m_CurrentHp <= 0 )
        {
            SceneManagement.Instance.ChangeLevel("Death");
        }
    }

    private IEnumerator BombDelay()
    {
        yield return new WaitForSeconds(3.8f);
        m_CanPlaceBomb = true;
    }

    private void FixedUpdate()
    {
        if (m_IsMoving)
        {
            m_PercentageCompletion += Time.fixedDeltaTime * m_CurrentSpeed;
            m_PercentageCompletion = Mathf.Clamp(m_PercentageCompletion, 0f, 1f);

            transform.position = Vector3.Lerp(m_InitialPos, m_WantedPos, m_PercentageCompletion);

            if (m_PercentageCompletion >= 1)
            {
                m_IsMoving = false;
            }
        }
    }
}
