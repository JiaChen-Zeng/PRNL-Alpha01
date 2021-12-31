using BulletHell;
using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// 敵の視野に入ってから攻撃を始めることを制御する
/// </summary>
public abstract class EnemyAI : MonoBehaviour
{
    /// <summary>
    /// 弾幕パターンが事前に設定しておいたプレハブを置くリスト
    /// </summary>
    [SerializeField] private GameObject[] m_danmakuList;

    /// <summary>
    /// 弾幕パターンリストの中の弾幕のそれぞれの持続時間
    /// </summary>
    [SerializeField] private float[] m_danmakuDurations;

    /// <summary>
    /// 現在発射している弾幕パターンの添え字。-1 は現在発射していないことを意味する。
    /// </summary>
    private int m_currentDanmakuIndex = -1;
    private IEnumerator m_currentDanmakuRoutine;

    protected abstract GameObject GetFovObject();

    /// <summary>
    /// 敵の視野をエディター内で表示させる用
    /// </summary>
    protected abstract void OnDrawGizmosSelected();

    protected void Awake()
    {
        InitFOV();
        InitDanmakus();
    }
    
    private void InitFOV()
    {
        var fov = GetFovObject().GetComponent<EnemyFOV>();
        fov.OnEnterFOV.AddListener(OnEnterFOV);
        fov.OnExitFOV.AddListener(OnExitFOV);
    }

    private void InitDanmakus()
    {
        m_danmakuList = m_danmakuList.Select(danmaku => Instantiate(danmaku, transform)).ToArray();
    }

    private void StartDanmaku(Transform playerTransform)
    {
        SetDanmakuTarget(playerTransform);
        m_currentDanmakuRoutine = DanmakuRoutine();
        if (m_danmakuDurations.Length > 0) StartCoroutine(m_currentDanmakuRoutine);
        else
        {
            m_currentDanmakuIndex = 0;
            SetDanmakuEnabled(m_currentDanmakuIndex, true);
        }
    }

    private void StopDanmaku()
    {
        SetDanmakuEnabled(m_currentDanmakuIndex, false);
        m_currentDanmakuIndex = -1;
        StopCoroutine(m_currentDanmakuRoutine);
    }

    private IEnumerator DanmakuRoutine()
    {
        m_currentDanmakuIndex = 0;
        SetDanmakuEnabled(m_currentDanmakuIndex, true);
        while (true)
        {
            yield return new WaitForSeconds(m_danmakuDurations[m_currentDanmakuIndex]);
            SetDanmakuEnabled(m_currentDanmakuIndex, false);
            m_currentDanmakuIndex = (m_currentDanmakuIndex + 1) % m_danmakuList.Length;
            SetDanmakuEnabled(m_currentDanmakuIndex, true);
        }
    }

    private void SetDanmakuTarget(Transform playerTransform)
    {
        foreach (var danmaku in m_danmakuList)
        {
            foreach (var emitter in danmaku.GetComponents<ProjectileEmitterAdvanced>())
            {
                emitter.Target = playerTransform;
            }
        }
    }

    private void SetDanmakuEnabled(int index, bool enabled)
    {
        foreach (var emitter in m_danmakuList[index].GetComponents<ProjectileEmitterAdvanced>())
        {
            emitter.AutoFire = enabled;
        }
    }

    private void OnEnterFOV(GameObject player)
    {
        StartDanmaku(player.transform);
    }

    private void OnExitFOV(GameObject player)
    {
        StopDanmaku();
    }
}