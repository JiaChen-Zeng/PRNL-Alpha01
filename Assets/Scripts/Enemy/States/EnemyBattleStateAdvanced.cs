using BulletHell;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading;
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
    private CancellationTokenSource danmakuCts;

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

    protected async override void StartDanmaku()
    {
        danmakuCts = new CancellationTokenSource();
        await SwitchDanmakuContinuously(danmakuCts.Token);
    }

    protected override void StopDanmaku()
    {
        danmakuCts.Cancel();
        SetDanmakuEnabled(currentDanmakuIndex, false);
        currentDanmakuIndex = -1;
    }

    private async UniTask SwitchDanmakuContinuously(CancellationToken token)
    {
        currentDanmakuIndex = 0;
        SetDanmakuEnabled(currentDanmakuIndex, true);
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(danmakuDurations[currentDanmakuIndex]));
            if (token.IsCancellationRequested) return;

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
