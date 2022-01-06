using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> soundList;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnButtonClickSE()
    {
        audioSource.PlayOneShot(soundList[0]);
    }
    public void OnClearSE()
    {
        audioSource.PlayOneShot(soundList[1]);
    }
    public void OnWalkSE()
    {
        audioSource.PlayOneShot(soundList[2]);
    }
    public void OnJumpSE()
    {
        audioSource.PlayOneShot(soundList[3]);
    }
    public void OnShieldJumpSE()
    {
        audioSource.PlayOneShot(soundList[4]);
    }
    public void OnLandingSE()
    {
        audioSource.PlayOneShot(soundList[5]);
    }
    public void OnShieldDefenseSE()
    {
        audioSource.PlayOneShot(soundList[6]);
    }
    public void OnShieldAttackSE()
    {
        audioSource.PlayOneShot(soundList[7]);
    }
    public void OnReceivedDamageSE()
    {
        audioSource.PlayOneShot(soundList[8]);
    }
    public void OnDeathSE()
    {
        audioSource.PlayOneShot(soundList[9]);
    }
    public void OnThinkingSE()
    {
        audioSource.PlayOneShot(soundList[10]);
    }
    public void OnEnemyLandingSE()
    {
        audioSource.PlayOneShot(soundList[11]);
    }
    public void OnBulletLaunchSE()
    {
        audioSource.PlayOneShot(soundList[12]);
    }
    public void OnStunSE()
    {
        audioSource.PlayOneShot(soundList[13]);
    }
}
