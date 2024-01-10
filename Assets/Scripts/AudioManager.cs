using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] seClips;
    [SerializeField] private AudioClip[] bgmClips;

    private AudioSource[] seSources = new AudioSource[13];
    private AudioSource[] bgmSources = new AudioSource[2];

    [SerializeField] private float seVolume = 0.2f;
    [SerializeField] private float bgmVolume = 0.1f;
    [SerializeField] private bool mute = false;

    private bool isCrossFading;

    // クロスフェード時間
    [SerializeField] private const float crossFadeTime = 1.0f;

    public enum SE_Type
    {
        ButtonClick = 0,
        Clear = 1,
        Walk = 2,
        Jump = 3,
        ShieldJump = 4,
        Landing = 5,
        ShieldDefense = 6,
        ShieldAttack = 7,
        ReceivedDamage = 8,
        Death = 9,
        Thinking = 10,
        EnemyLanding = 11,
        BulletLaunch = 12,
        Stun = 13
    }

    public enum BGM_Type
    {
        NormalPlay = 0,
        BossBattle = 1,
        Menu = 2,

        Silence = 999,
    }

    private void Awake()
    {
        // BGM用 AudioSource追加
        bgmSources[0] = gameObject.AddComponent<AudioSource>();
        bgmSources[1] = gameObject.AddComponent<AudioSource>();

        bgmSources[0].volume = bgmVolume;
        bgmSources[1].volume = bgmVolume;

        // SE用 AudioSource追加
        for (int i = 0; i < seSources.Length; i++)
        {
            seSources[i] = gameObject.AddComponent<AudioSource>();
        }

        foreach (AudioSource source in seSources)
        {
            source.volume = seVolume;
        }
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    public void PlayBGM(BGM_Type bgmType, bool loopFlg = true)
    {
        // BGMなしの状態にする場合
        if ((int)bgmType == 999)
        {
            StopBGM();
            return;
        }

        var index = (int)bgmType;
        if (index < 0 || bgmClips.Length <= index)
        {
            return;
        }

        // 同じBGMの場合は何もしない
        if (bgmSources[0].clip != null && bgmSources[0].clip == bgmClips[index])
        {
            return;
        }
        else if (bgmSources[1].clip != null && bgmSources[1].clip == bgmClips[index])
        {
            return;
        }

        // フェードでBGM開始
        if (bgmSources[0].clip == null && bgmSources[1].clip == null)
        {
            bgmSources[0].loop = loopFlg;
            bgmSources[0].clip = bgmClips[index];
            bgmSources[0].Play();
        }
        else
        {
            // クロスフェード処理
            StartCoroutine(CrossFadeChangeBMG(index, loopFlg));
        }
    }

    /// <summary>
    /// BGMのクロスフェード処理
    /// </summary>
    /// <param name="index">AudioClipの番号</param>
    /// <param name="loopFlg">ループ設定。ループしない場合だけfalse指定</param>
    private IEnumerator CrossFadeChangeBMG(int index, bool loopFlg)
    {
        isCrossFading = true;
        if (bgmSources[0].clip != null)
        {
            // [0]が再生されている場合、[0]の音量を徐々に下げて、[1]を新しい曲として再生
            bgmSources[1].volume = bgmVolume;
            bgmSources[1].clip = bgmClips[index];
            bgmSources[1].loop = loopFlg;
            bgmSources[1].Play();
            // bgmSources[0].DOFade(0, crossFadeTime).SetEase(Ease.Linear);

            yield return new WaitForSeconds(crossFadeTime);
            bgmSources[0].Stop();
            bgmSources[0].clip = null;
        }
        else
        {
            // [1]が再生されている場合、[1]の音量を徐々に下げて、[0]を新しい曲として再生
            bgmSources[0].volume = bgmVolume;
            bgmSources[0].clip = bgmClips[index];
            bgmSources[0].loop = loopFlg;
            bgmSources[0].Play();
            // bgmSources[1].DOFade(0, crossFadeTime).SetEase(Ease.Linear);

            yield return new WaitForSeconds(crossFadeTime);
            bgmSources[1].Stop();
            bgmSources[1].clip = null;
        }

        isCrossFading = false;
    }

    /// <summary>
    /// BGM完全停止
    /// </summary>
    public void StopBGM()
    {
        bgmSources[0].Stop();
        bgmSources[1].Stop();
        bgmSources[0].clip = null;
        bgmSources[1].clip = null;
    }

    /// <summary>
    /// SE再生
    /// </summary>
    public void PlaySE(SE_Type seType)
    {
        int index = (int)seType;
        if (index < 0 || seClips.Length <= index)
        {
            return;
        }

        // 再生中ではないAudioSouceをつかってSEを鳴らす
        foreach (AudioSource source in seSources)
        {
            if (false == source.isPlaying)
            {
                source.clip = seClips[index];
                source.Play();
                return;
            }
        }
    }

    /// <summary>
    /// SE停止
    /// </summary>
    public void StopSE()
    {
        // 全てのSE用のAudioSouceを停止する
        foreach (AudioSource source in seSources)
        {
            source.Stop();
            source.clip = null;
        }
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void MuteBGM()
    {
        bgmSources[0].Stop();
        bgmSources[1].Stop();
    }

    /// <summary>
    /// 一時停止した同じBGMを再生(再開)
    /// </summary>
    public void ResumeBGM()
    {
        bgmSources[0].Play();
        bgmSources[1].Play();
    }
}