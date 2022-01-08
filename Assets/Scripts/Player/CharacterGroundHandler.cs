using UnityEngine;

/// <summary>
/// 主人公の地面とのコリジョン、セーブポイントとの連携を処理する
/// </summary>
public class CharacterGroundHandler : MonoBehaviour
{
    private static bool CollidedWithGround(Collision2D collision)
    {
        return 0 < (collision.gameObject.layer & LayerMask.NameToLayer("Ground")) && collision.enabled;
    }

    public bool Grounded { get => Ground; }

    private Collider2D collider;

    /// <summary>
    /// 今立っているプラットフォームか壁
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
        if (!Ground || !CanFallThrough()) return;

        Physics2D.IgnoreCollision(collider, Ground);
    }

    public void ReactivatePreviousGround()
    {
        if (!prevGround) return;

        Physics2D.IgnoreCollision(collider, prevGround, false);
    }

    private bool CanFallThrough()
    {
        return !(Ground &&
           ((Ground.GetComponent<SavePoint>()?.Level == 1) // 一番下のプラットフォームも落ちない
        || !Ground.CompareTag("Platform"))); // プラットフォーム以外（壁）は落ちない
    }

    private void HandleSavePoint(Collision2D collision)
    {
        var savePoint = collision.gameObject.GetComponent<SavePoint>();
        if (!savePoint) return;

        // TODO: ステージクリア表示、頂上だとゲームクリア
        LastSavePoint = savePoint;

        // TODO: 回復エフェクト
        GameManager.INSTANCE.RecoverPlayerHP();

        TimeManager.INSTANCE.ResetTimer();
        TimeManager.INSTANCE.StartTimer();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (CollidedWithGround(collision))
        {
            ReactivatePreviousGround();
            Ground = collision.collider;
            HandleSavePoint(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (CollidedWithGround(collision))
        {
            prevGround = Ground;
            Ground = null;
        }
    }
}
