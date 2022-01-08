using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UnityEngine;

/// <summary>
/// 主人公を追撃する機能
/// </summary>
public class PlayerChaser
{
    private readonly EnemyBattleStateAdvanced state;

    /// <summary>
    /// 何秒ごとに一回ジャンプする
    /// </summary>
    private readonly float chaseInterval;

    private CancellationTokenSource chaseCts;

    public PlayerChaser(EnemyBattleStateAdvanced state, float chaseInterval)
    {
        this.state = state;
        this.chaseInterval = chaseInterval;
    }

    public async void StartChasePlayer()
    {
        chaseCts = new CancellationTokenSource();
        await ChasePlayerTask(chaseCts.Token);
    }

    public void StopChasePlayer()
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
        var platformTracker = GameObject.Find("PlayerLeadPlatformTracker").GetComponent<PlatformTracker>();
        var platform = platformTracker.SelectLeadPlatform(state.gameObject, state.Ai.IdleState.FovObject.GetComponent<Collider2D>());
        if (!platform) return;

        // TEMP: 簡単にプラットフォームまで移動させる。後でちゃんとジャンプするように変える必要がある。
        var pos = platform.transform.position;
        pos.y -= 1.3f;
        var pWidth = Math.Min(platform.transform.localScale.x - 1, 10); // 横幅 20 のプラットフォームで画面外に行ってしまわないように
        pos.x += UnityEngine.Random.Range(-pWidth / 2, pWidth / 2);
        state.transform.DOMove(pos, 1);
    }
}