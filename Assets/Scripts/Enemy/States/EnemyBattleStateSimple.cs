using BulletHell;
using UnityEngine;

/// <summary>
/// 雑魚用。弾幕パターンは一個だけ。
/// </summary>
public class EnemyBattleStateSimple : EnemyBattleState
{
    [SerializeField] private ProjectileEmitterAdvanced danmaku;

    protected override void InitDanmakus()
    {
        danmaku = Instantiate(danmaku, transform);
    }

    public override void SetDanmakuTarget(Transform playerTransform)
    {
        danmaku.Target = playerTransform;
    }

    protected override void StartDanmaku()
    {
        danmaku.AutoFire = true;
    }

    protected override void StopDanmaku()
    {
        danmaku.AutoFire = false;
    }
}
