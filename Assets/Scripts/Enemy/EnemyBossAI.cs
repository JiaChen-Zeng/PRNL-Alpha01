using UnityEngine;

/// <summary>
/// ボスの AI は追撃機能もある
/// </summary>
public class EnemyBossAI : EnemyAI
{
    /// <summary>
    /// ボスの固有領域
    /// </summary>
    [SerializeField] private EnemyBossFOV m_intrinsicArea;

    protected override GameObject GetFovObject()
    {
        return m_intrinsicArea.gameObject;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireCube(m_intrinsicArea.transform.position, m_intrinsicArea.transform.localScale);
    }
}
