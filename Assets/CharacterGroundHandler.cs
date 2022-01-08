using UnityEngine;

/// <summary>
/// 主人公の地面とのコリジョン、セーブポイントとの連携を処理する
/// </summary>
public class CharacterGroundHandler : MonoBehaviour
{
    public bool Grounded { get => Ground; }

    private Collider2D collider;

    /// <summary>
    /// 今立っているプラットフォーム
    /// </summary>
    private Collider2D Ground { get; set; }
    private Collider2D prevGround;

    public SavePoint LastSavePoint { get; private set; }

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
    private void HandleSavePoint(Collision2D collision)
    {
        var savePoint = collision.gameObject.GetComponent<SavePoint>();
        if (!savePoint) return;

        // TODO: ステージクリア表示、頂上だとゲームクリア
        LastSavePoint = savePoint;

        // TODO: 回復エフェクト
        GameManager.INSTANCE.RecoverPlayerHP(); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            ReactivatePreviousGround();
            Ground = collision.collider;
            HandleSavePoint(collision);
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
