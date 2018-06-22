using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float m_Speed;
    public float m_CurrentHp = 10;

    private Player m_Player;
    private int m_CurrentRow;
    private int m_CurrentCol;
    private List<GameObject> m_Bombs = new List<GameObject>();
    private bool m_IsMoving = false;
    private Vector2 m_InitialPos;
    private Vector2 m_WantedPos;
    private float m_PercentageCompletion;
    private float m_InputX;
    private float m_InputY;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private GameObject m_BombPrefab;

    public int CurrentRow
    {
        get { return m_CurrentRow; }
    }
    public int CurrentCol
    {
        get { return m_CurrentCol; }
    }

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    public void Setup(int aRow, int aCol)
    {
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
                LevelGenerator.Instance.GetTileTypeAtPos(m_CurrentRow, m_CurrentCol + (int)askMoveHorizontal) == ETileType.Floor)
            {
                m_IsMoving = true;
                m_PercentageCompletion = 0f;

                m_InitialPos = transform.position;
                m_WantedPos = LevelGenerator.Instance.GetPositionAt(m_CurrentRow, m_CurrentCol + (int)askMoveHorizontal);

                m_CurrentCol += (int)askMoveHorizontal;
            }
            else if (askMoveVertical != 0 &&
                LevelGenerator.Instance.GetTileTypeAtPos(m_CurrentRow - (int)askMoveVertical, m_CurrentCol) == ETileType.Floor)
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Bombs.Add(m_BombPrefab);
            GameObject Instance = Instantiate(m_BombPrefab, transform.position, Quaternion.identity);
        }
    }

    private void FixedUpdate()
    {
        if (m_IsMoving)
        {
            m_PercentageCompletion += Time.fixedDeltaTime * m_Speed;
            m_PercentageCompletion = Mathf.Clamp(m_PercentageCompletion, 0f, 1f);

            transform.position = Vector3.Lerp(m_InitialPos, m_WantedPos, m_PercentageCompletion);

            if (m_PercentageCompletion >= 1)
            {
                m_IsMoving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D aOther)
    {
        if (aOther.gameObject.layer == LayerMask.GetMask("BombBlast"))
        {
            Debug.Log("Hit");
            m_CurrentHp -= 1;
        }
    }
}
