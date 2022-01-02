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

    /// <summary>
    /// 主人公を追撃する機能
    /// </summary>
    private PlayerChaser playerChaser;

    [SerializeField] private float chaseInterval = 3;

    public override void Enter()
    {
        base.Enter();
        playerChaser.StartChasePlayer();
    }

    public override void Exit()
    {
        base.Exit();
        playerChaser.StopChasePlayer();
    }

    protected override void Awake()
    {
        base.Awake();
        playerChaser = new PlayerChaser(this, chaseInterval);
    }

    protected override void InitDanmakus()
    {
        danmakuList = danmakuList.Select(d => d ? Instantiate(d, transform) : null).ToArray();
    }

    public override void SetDanmakuTarget(Transform playerTransform)
    {
        foreach (var danmaku in danmakuList)
        {
            if (!danmaku) continue;

            foreach (var emitter in danmaku.GetComponents<ProjectileEmitterAdvanced>())
            {
                emitter.Target = playerTransform;
            }
        }
    }

    protected async override void StartDanmaku()
    {
        danmakuCts = new CancellationTokenSource();
        await SwitchDanmakuTask(danmakuCts.Token);
    }

    protected override void StopDanmaku()
    {
        danmakuCts.Cancel();
        SetDanmakuEnabled(currentDanmakuIndex, false);
        currentDanmakuIndex = -1;
    }

    private async UniTask SwitchDanmakuTask(CancellationToken token)
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
        if (!danmakuList[index]) return;

        foreach (var emitter in danmakuList[index].GetComponents<ProjectileEmitterAdvanced>())
        {
            emitter.AutoFire = enabled;
        }
    }
}
