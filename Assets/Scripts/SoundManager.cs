using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> soundList;
    private AudioSource audioSource;

    public enum AudioClipType
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

    public AudioClipType _audioClipType;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnSE(AudioClipType _audioClipType)
    {
        audioSource.PlayOneShot(soundList[(int)_audioClipType]);
    }
}