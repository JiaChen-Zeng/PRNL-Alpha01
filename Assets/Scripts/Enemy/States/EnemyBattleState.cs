using UnityEngine;

public abstract class EnemyBattleState : EnemyState
{
    public override void Enter()
    {
        StartDanmaku();
    }

    public override void Exit()
    {
        StopDanmaku();
    }

    protected override void Awake()
    {
        base.Awake();
        InitDanmakus();
    }

    protected abstract void InitDanmakus();
    protected abstract void StartDanmaku();
    protected abstract void StopDanmaku();
    public abstract void SetDanmakuTarget(Transform playerTransform);
}
