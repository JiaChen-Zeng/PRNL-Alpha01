using BulletHell;
using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// 敵の視野に入ってから攻撃を始めることを制御する
/// </summary>
public class EnemyAttackController : MonoBehaviour
{
    /// <summary>
    /// 敵の種類によって攻撃の AI が違う
    /// </summary>
    [SerializeField] private bool m_isBoss;

    /// <summary>
    /// ボスの固有領域
    /// </summary>
    [SerializeField] private EnemyBossFOV m_intrinsicArea;

    /// <summary>
    /// 敵の視野半径
    /// </summary>
    [SerializeField] private float m_fieldOfView = 5;
    private GameObject m_fov;

    [SerializeField] private GameObject[] m_danmakuList;

    private void Awake()
    {
        if (m_isBoss) InitBossFOV();
        else InitNormalFOV();
        InitDanmakus();
    }

    private void InitBossFOV()
    {
        m_intrinsicArea.OnEnterFOV.AddListener(OnEnterFOV);
        m_intrinsicArea.OnExitFOV.AddListener(OnExitFOV);
    }

    private void InitNormalFOV()
    {
        m_fov = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyNormalFOV"), transform);
        var fov = m_fov.GetComponent<EnemyNormalFOV>();
        fov.Radius = m_fieldOfView;
        fov.OnEnterFOV.AddListener(OnEnterFOV);
        fov.OnExitFOV.AddListener(OnExitFOV);
    }

    private void InitDanmakus()
    {
        m_danmakuList = m_danmakuList.Select(danmaku => Instantiate(danmaku, transform)).ToArray();
    }

    private void OnEnterFOV(GameObject player)
    {
        foreach (var danmaku in m_danmakuList)
        {
            foreach (var emitter in danmaku.GetComponents<ProjectileEmitterAdvanced>())
            {
                emitter.AutoFire = true;
                emitter.Target = player.transform;
            }
        }
    }

    private void OnExitFOV(GameObject player)
    {
        foreach (var danmaku in m_danmakuList)
        {
            foreach (var emitter in danmaku.GetComponents<ProjectileEmitterAdvanced>())
            {
                emitter.AutoFire = false;
            }
        }
    }
}
