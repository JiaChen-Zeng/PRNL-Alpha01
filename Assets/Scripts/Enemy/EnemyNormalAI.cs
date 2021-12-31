using UnityEngine;

public class EnemyNormalAI : EnemyAI
{
    /// <summary>
    /// 敵の視野半径
    /// </summary>
    [SerializeField] private float m_fieldOfView = 5;

    protected override GameObject GetFovObject()
    {
        var fovObject = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyNormalFOV"), transform);
        fovObject.GetComponent<EnemyNormalFOV>().Radius = m_fieldOfView;
        return fovObject;
    }

    protected override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_fieldOfView);
    }
}
