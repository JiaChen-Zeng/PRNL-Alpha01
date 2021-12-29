using System.Collections;
using System.Collections.Generic;
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

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
    private bool m_Grounded;            // Whether or not the player is grounded.
    //const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
    private Rigidbody2D m_Rigidbody;
    private bool m_FacingRight = true;  // For determining which way the player is currently facing.
    public bool FacingRight { get => m_FacingRight; private set => m_FacingRight = value; }
    private Vector2 m_Velocity = Vector3.zero;
    private int mAirJumpCount = 0;
    private float mHorizontalMove = 0f;
    private bool mCanShieldJump = false;
    private Vector3 mNewPosition = new Vector3();

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

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();

        m_airJumpMaxCount = 1;
    }

    private void OnEnable()
    {
        //EventManager.pInstance.OnShieldCollision += OnShieldCollision;
        EventManager.pInstance.OnDamageReceived += OnDamageReceived;
    }

    private void OnDamageReceived(Collider2D bullet)
    {
        //DoDamage(damage from the bullet)
    }

    public void DoDamage(float damage)
    {
        //reduce the hitpoints
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

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;


        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        m_Grounded = Physics2D.OverlapCircle(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        if (!wasGrounded)
            OnLandEvent.Invoke();

        Move(mHorizontalMove);
    }


    public void Move(float move)
    {
        
        //only control the player if grounded or airControl is turned on
        if (m_Grounded || m_AirControl)
        {
            // Move the character by finding the target velocity
            Vector2 targetVelocity = new Vector2(move * m_moveSpeed, m_Rigidbody.velocity.y);
            // And then smoothing it out and applying it to the character
            m_Rigidbody.velocity = Vector2.SmoothDamp(m_Rigidbody.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            // If the input is moving the player right and the player is facing left...
            FlipByMovementAndShield(move);
        }
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

    private void Update()
    {
        mHorizontalMove = Input.GetAxisRaw("Horizontal");
        ShieldJumpCheck();
        if (m_Grounded)
        {
            mAirJumpCount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && m_Grounded)
        {
            m_Rigidbody.velocity = Vector3.up * m_JumpForce;

        }
        else if (mCanShieldJump && Input.GetKeyDown(KeyCode.Space))
            DoShieldJump();
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (mAirJumpCount < m_airJumpMaxCount)
            {
                m_Rigidbody.velocity = Vector3.up * m_JumpForce;
                mAirJumpCount++;
            }
        }
    }

    private void DoShieldJump()
    {
        m_Rigidbody.velocity = Vector3.up * m_ShieldJumpForce;
        Debug.LogFormat("Shield jump");
    }

    private void ShieldJumpCheck()
    {
        if (!m_Grounded && m_Rigidbody.velocity.y < 0)
        {
            mCanShieldJump = Physics2D.OverlapCircle(m_shieldJumpCheck.position, k_GroundedRadius, m_WhatIsGround);
        }
        else
            mCanShieldJump = false;

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

    private void OnDestroy()
    {
        //EventManager.pInstance.OnShieldCollision -= OnShieldCollision;
        EventManager.pInstance.OnDamageReceived -= OnDamageReceived;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(m_shieldJumpCheck.position, k_GroundedRadius);
    }
}
