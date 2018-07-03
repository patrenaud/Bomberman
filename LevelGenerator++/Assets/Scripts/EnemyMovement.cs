using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private int m_CurrentRow;
    private int m_CurrentCol;
    private bool m_IsMoving = false;
    private Vector2 m_InitialPos;
    private Vector2 m_WantedPos;
    private float m_PercentageCompletion;
    private float m_CurrentSpeed = 2f;
    private float m_InputX;
    private float m_InputY;
    [SerializeField] private Animator m_Animator;

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
                LevelGenerator.Instance.GetTileTypeAtIndex(m_CurrentRow, m_CurrentCol + (int)askMoveHorizontal) == ETileType.Floor)
            {
                m_IsMoving = true;
                m_PercentageCompletion = 0f;

                m_InitialPos = transform.position;
                m_WantedPos = LevelGenerator.Instance.GetPositionAt(m_CurrentRow, m_CurrentCol + (int)askMoveHorizontal);
                if ((int)askMoveHorizontal < 0)
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }

                m_CurrentCol += (int)askMoveHorizontal;
            }
            else if (askMoveVertical != 0 &&
                LevelGenerator.Instance.GetTileTypeAtIndex(m_CurrentRow - (int)askMoveVertical, m_CurrentCol) == ETileType.Floor)
            {
                m_IsMoving = true;
                m_PercentageCompletion = 0f;

                m_InitialPos = transform.position;
                m_WantedPos = LevelGenerator.Instance.GetPositionAt(m_CurrentRow - (int)askMoveVertical, m_CurrentCol);
                m_CurrentRow -= (int)askMoveVertical;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                Debug.Log(m_CurrentCol);
                Debug.Log(m_CurrentRow);
            }
        }

        // Ceci contrôle les animations du Player
        m_InputX = Input.GetAxisRaw("Horizontal");
        m_InputY = Input.GetAxisRaw("Vertical");
        m_Animator.SetInteger("MoveHorizontal", (int)m_InputX);
        m_Animator.SetInteger("MoveVertical", (int)m_InputY);

        // Ceci, normalement, détruit l'ennemi lorsqu'il est sur la même case que la Player
        if (m_CurrentRow == LevelGenerator.Instance.m_PlayerPrefab.m_CurrentRow && m_CurrentCol == LevelGenerator.Instance.m_PlayerPrefab.m_CurrentCol)
        {
            Destroy(gameObject);
        }

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
