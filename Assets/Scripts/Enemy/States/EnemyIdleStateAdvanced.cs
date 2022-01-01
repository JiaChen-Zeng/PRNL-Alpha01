using UnityEngine;

/// <summary>
/// 主にボス用。固有領域がある。
/// </summary>
public class EnemyIdleStateAdvanced : EnemyIdleState
{
    /// <summary>
    /// ボスの固有領域
    /// </summary>
    [SerializeField] private EnemyBossFOV intrinsicArea;

    protected override GameObject GetFovObject()
    {
        return intrinsicArea.gameObject;
    }

    protected override void OnDrawGizmosSelected()
    {
        if (!intrinsicArea) return;

        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(intrinsicArea.transform.position, intrinsicArea.transform.localScale);
    }
}
