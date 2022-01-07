using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager INSTANCE { get; private set; }

    [field: SerializeField] public int PlayerInitialHP { get; private set; } = 5;
    private int playerHp;
    public int PlayerHp
    {
        get => playerHp;
        set
        {
            playerHp = value;
            hpIndicator.HP = value;
        }
    }

    private HPIndicator hpIndicator;

    public GameManager()
    {
        if (INSTANCE) throw new InvalidOperationException("GameManagerのインスタンスは一つのみ");

        INSTANCE = this;
    }

    private void Awake()
    {
        initHP();
    }

    private void initHP()
    {
        hpIndicator = FindObjectOfType<HPIndicator>();
        hpIndicator.InitHPObjects();
        playerHp = PlayerInitialHP;
    }
}
