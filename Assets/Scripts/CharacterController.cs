using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float m_ShieldJumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float m_moveSpeed = 10f;                          // Amount of force added when the player jumps.
    [SerializeField] private int m_airJumpMaxCount = 1;
    [Range(0, 10)] [SerializeField] private float m_fallMultiplier = 2f;         
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Transform m_shieldJumpCheck;
    [SerializeField] private float m_xMinConstraint;
    [SerializeField] private float m_xMaxConstraint;
    [SerializeField] private bool m_flip = true;

    [Header("Stats")]
    [SerializeField] private float m_hitPoints;
    public bool FacingRight { get; private set; } = true;

    private Rigidbody2D m_Rigidbody;
    private Vector2 m_Velocity = Vector3.zero;

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool grounded;            // Whether or not the player is grounded.
    //const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up

    private int mAirJumpCount = 0;
    private bool mCanShieldJump = false;
    private bool canAirJump { get => mAirJumpCount < m_airJumpMaxCount; }

    private float mHorizontalMove = 0f;

    /// <summary>
    /// これで盾の位置調整の状態を取得する。主人公の向きの制御は盾の位置調整の状態によって、主人公の移動ではなく盾の調整で制御されることがあるため。
    /// </summary>
    private ShieldController m_sc;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        m_sc = GetComponentInChildren<ShieldController>();
        m_Rigidbody = GetComponent<Rigidbody2D>();

        OnLandEvent ??= new UnityEvent();
    }

    private void OnEnable()
    {
        //EventManager.pInstance.OnShieldCollision += OnShieldCollision;
        EventManager.pInstance.OnDamageReceived += OnDamageReceived;
    }

    private void Update()
    {
        mHorizontalMove = Input.GetAxisRaw("Horizontal");
        HandleJump();
    }

    private void FixedUpdate()
    {
        Move(mHorizontalMove);
    }

    private void OnDestroy()
    {
        //EventManager.pInstance.OnShieldCollision -= OnShieldCollision;
        EventManager.pInstance.OnDamageReceived -= OnDamageReceived;
    }

    public void Move(float move)
    {
        //only control the player if grounded or airControl is turned on
        if (grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector2 targetVelocity = new Vector2(move * m_moveSpeed, m_Rigidbody.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody.velocity = Vector2.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            // If the input is moving the player right and the player is facing left...
            FlipByMovementAndShield(move);
        }
        var mNewPosition = new Vector3();
        mNewPosition = m_Rigidbody.position;
        mNewPosition.x = Mathf.Clamp(mNewPosition.x, m_xMinConstraint, m_xMaxConstraint);
        m_Rigidbody.position = mNewPosition;
    }

    /// <summary>
    /// 主人公の移動状態と盾の位置調整状態によって主人公を反転させる
    /// </summary>
    /// <param name="dx">x 軸移動量</param>
    private void FlipByMovementAndShield(float dx)
    {
        if (!m_flip) return;

        if (m_sc.IsControlling) // プレイヤーが盾の位置を制御しているとき
        {
            // 盾の位置によって反転するかを決める
            if (!m_sc.FacingFront)
            {
                Flip();
                m_sc.Flip();
            }
        }
        else
        {
            // 主人公の移動によって反転するかを決める
            if (dx > 0 && !FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (dx < 0 && FacingRight)
            {
                // ... flip the player.
                Flip();
            }
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        FacingRight = !FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void HandleJump()
    {
        ShieldJumpCheck();
        if (grounded) mAirJumpCount = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                Debug.Log("DoJump");
                DoJump();
            }
            else if (mCanShieldJump)
            {
                Debug.Log("DoShieldJump");
                DoShieldJump();
            }
            else if (canAirJump)
            {
                Debug.Log("DoAirJump");
                DoAirJump();
            }
        }
    }

    private void DoJump()
    {
        m_Rigidbody.velocity = Vector3.up * m_JumpForce;
    }
    private void DoAirJump()
    {
        DoJump();
        mAirJumpCount++;
    }

    private void DoShieldJump()
    {
        m_Rigidbody.velocity = Vector3.up * m_ShieldJumpForce;
    }

    private void ShieldJumpCheck()
    {
        if (!grounded && m_Rigidbody.velocity.y < 0)
        {
            mCanShieldJump = Physics2D.OverlapCircle(m_shieldJumpCheck.position, k_GroundedRadius, m_WhatIsGround);
        }
        else mCanShieldJump = false;
    }

    public void DoDamage(float damage)
    {
        //reduce the hitpoints
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            if (!grounded) OnLandEvent.Invoke();
            grounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            grounded = false;
        }
    }

    private void OnDamageReceived(Collider2D bullet)
    {
        //DoDamage(damage from the bullet)
    }

    private void OnShieldCollision(Collider2D collider, bool enter)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //if(enter && collider)
            //{
            //    DoShieldJump();
            //}
        }
        else
            mCanShieldJump = false;
        //Debug.LogFormat("On enter = {0}", enter);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_shieldJumpCheck.position, k_GroundedRadius);
    }
}
