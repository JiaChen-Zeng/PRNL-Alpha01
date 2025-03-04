﻿using UnityEngine;

/// <summary>
/// 主に雑魚敵用。視野が円形。
/// </summary>
public class EnemyIdleStateSimple : EnemyIdleState
{
    /// <summary>
    /// 敵の視野半径
    /// </summary>
    [SerializeField] private float fieldOfView = 5;
    private GameObject fovObject;

    public override GameObject FovObject
    {
        get
        {
            if (fovObject) return fovObject;

            fovObject = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyNormalFOV"), transform);
            fovObject.GetComponent<EnemyNormalFOV>().Radius = fieldOfView;
            return fovObject;
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.DrawWireSphere(transform.position, fieldOfView);
    }
}
