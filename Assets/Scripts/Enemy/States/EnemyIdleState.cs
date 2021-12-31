using UnityEngine;

public abstract class EnemyIdleState : EnemyState
{
    protected abstract GameObject GetFovObject();

    protected override void Awake()
    {
        base.Awake();
        InitFOV();
    }

    private void InitFOV()
    {
        var fov = GetFovObject().GetComponent<EnemyFOV>();
        fov.OnEnterFOV.AddListener(OnEnterFOV);
        fov.OnExitFOV.AddListener(OnExitFOV);
    }

    private void OnEnterFOV(GameObject player)
    {
        ai.BattleState.SetDanmakuTarget(player.transform);
        ai.State = ai.BattleState;
    }

    private void OnExitFOV(GameObject player)
    {
        ai.State = this;
    }

    /// <summary>
    /// 敵の視野をエディター内で表示させる用
    /// </summary>
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
    }
}
