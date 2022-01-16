using UnityEngine;

public class EnterAudio : MonoBehaviour
{
    [SerializeField] private AudioManager _audioManager;
    public void bgmChan0()
    {
        _audioManager.PlayBGM(AudioManager.BGM_Type.NormalPlay);
    }

    public void bgmChan1()
    {
        _audioManager.PlayBGM(AudioManager.BGM_Type.BossBattle);
    }
    
    public void bgmChan2()
    {
        _audioManager.PlayBGM(AudioManager.BGM_Type.Menu);
    }

    public void seChange0()
    {
        _audioManager.PlaySE(AudioManager.SE_Type.ButtonClick);
    }
    public void seChange1()
    {
        _audioManager.PlaySE(AudioManager.SE_Type.Jump);
    }
    public void seChange2()
    {
        _audioManager.PlaySE(AudioManager.SE_Type.ReceivedDamage);
    }
}
