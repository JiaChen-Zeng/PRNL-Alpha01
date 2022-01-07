using UnityEngine;

public class PlayerGroundHandler : MonoBehaviour
{
    public bool Grounded { get => Ground; }

    private Collider2D collider;
    /// <summary>
    /// 今立っているプラットフォーム
    /// </summary>
    private Collider2D Ground { get; set; }
    private Collider2D prevGround;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    public void FallThroughGround()
    {
        if (!Ground || (Ground && Ground.GetComponent<SavePoint>()?.Level == 1)) return; // 一番下のプラットフォームも落ちない

        Physics2D.IgnoreCollision(collider, Ground);
    }

    public void ReactivatePreviousGround()
    {
        if (!prevGround) return;

        Physics2D.IgnoreCollision(collider, prevGround, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            ReactivatePreviousGround();
            Ground = collision.collider;

            if (collision.gameObject.GetComponent<SavePoint>())
            {
                // TODO: 回復エフェクト、パートクリア、頂上だとゲームクリア
                GameManager.INSTANCE.RecoverPlayerHP();
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            prevGround = Ground;
            Ground = null;
        }
    }
}
