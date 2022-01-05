using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour
{
    const float GROUNDED_RADIUS = .2f; // Radius of the overlap circle to determine if grounded

    [SerializeField] private float jumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float shieldJumpForce = 400f;                          // Amount of force added when the player jumps.
    [SerializeField] private float moveSpeed = 10f;                          // Amount of force added when the player jumps.
    [SerializeField] private int airJumpMaxCount = 1;
    [Range(0, 10)] [SerializeField] private float fallMultiplier = 2f;         
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;  // How much to smooth out the movement
    [SerializeField] private bool airControl = false;                         // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask whatIsGround;                          // A mask determining what is ground to the character
    [SerializeField] private Transform shieldJumpCheck;
    [SerializeField] private float xMinConstraint;
    [SerializeField] private float xMaxConstraint;

    [Header("Stats")]
    [SerializeField] private float hitPoints;
    public bool FacingRight { get; private set; } = true;

    private Rigidbody2D rb;
    private Vector2 velocity = Vector3.zero;

    private bool grounded;            // Whether or not the player is grounded.
    //const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up

    private int airJumpCount = 0;
    private bool canShieldJump = false;
    private bool canAirJump { get => airJumpCount < airJumpMaxCount; }

    private float horizontalMove = 0f;

    /// <summary>
    /// これで盾の位置調整の状態を取得する。主人公の向きの制御は盾の位置調整の状態によって、主人公の移動ではなく盾の調整で制御されることがあるため。
    /// </summary>
    private ShieldController sc;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    private void Awake()
    {
        sc = GetComponentInChildren<ShieldController>();
        rb = GetComponent<Rigidbody2D>();

        OnLandEvent ??= new UnityEvent();
    }

    private void OnEnable()
    {
        //EventManager.pInstance.OnShieldCollision += OnShieldCollision;
        EventManager.pInstance.OnDamageReceived += OnDamageReceived;
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        HandleJump();
    }

    private void FixedUpdate()
    {
        Move(horizontalMove);
    }

    private void OnDestroy()
    {
        //EventManager.pInstance.OnShieldCollision -= OnShieldCollision;
        EventManager.pInstance.OnDamageReceived -= OnDamageReceived;
    }

    public void Move(float move)
    {
        //only control the player if grounded or airControl is turned on
        if (grounded || airControl)
        {
            // Move the character by finding the target velocity
            Vector2 targetVelocity = new Vector2(move * moveSpeed, rb.velocity.y);
            // And then smoothing it out and applying it to the character
            rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);
            // If the input is moving the player right and the player is facing left...
            FlipByMovementAndShield(move);
        }
        var newPosition = new Vector3();
        newPosition = rb.position;
        newPosition.x = Mathf.Clamp(newPosition.x, xMinConstraint, xMaxConstraint);
        rb.position = newPosition;
    }

    /// <summary>
    /// 主人公の移動状態と盾の位置調整状態によって主人公を反転させる
    /// </summary>
    /// <param name="dx">x 軸移動量</param>
    private void FlipByMovementAndShield(float dx)
    {
        if (sc.IsControlling) // プレイヤーが盾の位置を制御しているとき
        {
            // 盾の位置によって反転するかを決める
            if (!sc.FacingFront)
            {
                Flip();
                sc.Flip();
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
        if (grounded) airJumpCount = 0;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded)
            {
                Debug.Log("DoJump");
                DoJump();
            }
            else if (canShieldJump)
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
        rb.velocity = Vector3.up * jumpForce;
    }
    private void DoAirJump()
    {
        DoJump();
        airJumpCount++;
    }

    private void DoShieldJump()
    {
        rb.velocity = Vector3.up * shieldJumpForce;
    }

    private void ShieldJumpCheck()
    {
        if (!grounded && rb.velocity.y < 0)
        {
            canShieldJump = Physics2D.OverlapCircle(shieldJumpCheck.position, GROUNDED_RADIUS, whatIsGround);
        }
        else canShieldJump = false;
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
        else canShieldJump = false;
        //Debug.LogFormat("On enter = {0}", enter);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(shieldJumpCheck.position, GROUNDED_RADIUS);
    }
}
