using BulletHell;
using System.Linq;
using UnityEngine;

/// <summary>
/// 敵の視野に入ってから攻撃を始めることを制御する
/// </summary>
public class EnemyAttackController : MonoBehaviour
{
    /// <summary>
    /// 敵の視野半径
    /// </summary>
    [SerializeField] private float m_fieldOfView = 5;
    private GameObject m_fov;

    [SerializeField] private GameObject[] m_danmakuList;

    private void Awake()
    {
        InitFOV();
        InitDanmakus();
    }

    private void Update()
    {

    }

    private void InitFOV()
    {
        m_fov = Instantiate(Resources.Load<GameObject>("Prefabs/EnemyNormalFOV"), transform);
        var fov = m_fov.GetComponent<EnemyFOV>();
        fov.FOV = m_fieldOfView;
        fov.OnEnterFOV.AddListener(OnEnterFOV);
        fov.OnExitFOV.AddListener(OnExitFOV);
    }

    private void InitDanmakus()
    {
        m_danmakuList = m_danmakuList.Select(danmaku =>
        {
            danmaku = Instantiate(danmaku, transform);
            danmaku.GetComponent<ProjectileEmitterAdvanced>().AutoFire = false;
            return danmaku;
        }).ToArray();
    }

    private void OnEnterFOV(GameObject player)
    {
        Debug.Log("Enter FOV");
        foreach (var danmaku in m_danmakuList)
        {
            var emitter = danmaku.GetComponent<ProjectileEmitterAdvanced>();
            emitter.AutoFire = true;
            emitter.Target = player.transform;
        }
    }

    private void OnExitFOV(GameObject player)
    {
        Debug.Log("Exit FOV");
        foreach (var danmaku in m_danmakuList)
        {
            var emitter = danmaku.GetComponent<ProjectileEmitterAdvanced>();
            emitter.AutoFire = false;
        }
    }
}
