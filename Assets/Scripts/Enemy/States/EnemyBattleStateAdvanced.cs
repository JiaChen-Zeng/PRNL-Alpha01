using BulletHell;
using Cysharp.Threading.Tasks;
using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using DG.Tweening;

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

    [SerializeField] private float chaseInterval = 3;
    private CancellationTokenSource chaseCts;

    private PlatformTracker platformTracker;

    protected override void Awake()
    {
        base.Awake();
        platformTracker = GameObject.Find("PlayerLeadPlatformTracker").GetComponent<PlatformTracker>();
    }

    public override void Enter()
    {
        base.Enter();
        StartChasePlayer();
    }

    public override void Exit()
    {
        base.Exit();
        StopChasePlayer();
    }

    #region 弾幕

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
        foreach (var emitter in danmakuList[index].GetComponents<ProjectileEmitterAdvanced>())
        {
            emitter.AutoFire = enabled;
        }
    }

    #endregion

    private async void StartChasePlayer()
    {
        chaseCts = new CancellationTokenSource();
        await ChasePlayerTask(chaseCts.Token);
    }

    private void StopChasePlayer()
    {
        chaseCts.Cancel();
    }

    private async UniTask ChasePlayerTask(CancellationToken token)
    {
        while (true)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(chaseInterval));
            if (token.IsCancellationRequested) return;
            JumpToLeadPlayer();
        }
    }

    /// <summary>
    /// 主人公をリードするようにジャンプする
    /// </summary>
    private void JumpToLeadPlayer()
    {
        if (!platformTracker.HasPlatform) return;

        GameObject platform = platformTracker.GetRandomPlatform(); // ランダムに主人公から上のプラットフォームを取得

        // TEMP: 簡単にプラットフォームまで移動させる。後でちゃんとジャンプするように変える必要がある。
        var pos = platform.transform.position;
        pos.y -= 0.8f;
        var pWidth = Math.Min(platform.transform.localScale.x - 1, 10); // 横幅 20 のプラットフォームで画面外に行ってしまわないように
        pos.x += UnityEngine.Random.Range(-pWidth / 2, pWidth / 2);
        transform.DOMove(pos, 1);
    }
}
