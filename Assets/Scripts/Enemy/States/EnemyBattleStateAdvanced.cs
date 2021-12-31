using BulletHell;
using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// 主にボス用。複数パターンの弾幕を自動的に切り替えながら使える。
/// 追撃もできる。
/// </summary>
public class EnemyBattleStateAdvanced : EnemyBattleState
{
    /// <summary>
    /// 弾幕パターンが事前に設定しておいたプレハブを置くリスト
    /// </summary>
    [SerializeField] private GameObject[] danmakuList;

    /// <summary>
    /// 弾幕パターンリストの中の弾幕のそれぞれの持続時間
    /// </summary>
    [SerializeField] private float[] danmakuDurations;

    /// <summary>
    /// 現在発射している弾幕パターンの添え字。-1 は現在発射していないことを意味する。
    /// </summary>
    private int currentDanmakuIndex = -1;
    private IEnumerator currentDanmakuRoutine;

    protected override void InitDanmakus()
    {
        danmakuList = danmakuList.Select(danmaku => Instantiate(danmaku, transform)).ToArray();
    }

    public override void SetDanmakuTarget(Transform playerTransform)
    {
        foreach (var danmaku in danmakuList)
        {
            foreach (var emitter in danmaku.GetComponents<ProjectileEmitterAdvanced>())
            {
                emitter.Target = playerTransform;
            }
        }
    }

    protected override void StopDanmaku()
    {
        SetDanmakuEnabled(currentDanmakuIndex, false);
        currentDanmakuIndex = -1;
        StopCoroutine(currentDanmakuRoutine);
    }

    protected override void StartDanmaku()
    {
        currentDanmakuRoutine = DanmakuRoutine();
        StartCoroutine(currentDanmakuRoutine);
    }

    private IEnumerator DanmakuRoutine()
    {
        currentDanmakuIndex = 0;
        SetDanmakuEnabled(currentDanmakuIndex, true);
        while (true)
        {
            yield return new WaitForSeconds(danmakuDurations[currentDanmakuIndex]);
            SetDanmakuEnabled(currentDanmakuIndex, false);
            currentDanmakuIndex = (currentDanmakuIndex + 1) % danmakuList.Length;
            SetDanmakuEnabled(currentDanmakuIndex, true);
        }
    }

    private void SetDanmakuEnabled(int index, bool enabled)
    {
        foreach (var emitter in danmakuList[index].GetComponents<ProjectileEmitterAdvanced>())
        {
            emitter.AutoFire = enabled;
        }
    }
}
