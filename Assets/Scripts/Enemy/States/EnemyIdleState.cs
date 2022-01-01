using UnityEngine;

public abstract class EnemyIdleState : EnemyState
{
    public abstract GameObject FovObject { get; }

    protected override void Awake()
    {
        base.Awake();
        InitFOV();
    }

    private void InitFOV()
    {
        var fov = FovObject.GetComponent<EnemyFOV>();
        fov.OnEnterFOV.AddListener(OnEnterFOV);
        fov.OnExitFOV.AddListener(OnExitFOV);
    }

    private void OnEnterFOV(GameObject player)
    {
        Ai.BattleState.SetDanmakuTarget(player.transform);
        Ai.State = Ai.BattleState;
    }

    private void OnExitFOV(GameObject player)
    {
        Ai.State = this;
    }

    /// <summary>
    /// 敵の視野をエディター内で表示させる用
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
    }
}
