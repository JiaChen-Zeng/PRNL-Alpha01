using UnityEngine;

/// <summary>
/// 敵の視野に入ってから攻撃を始めることを制御する
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [HideInInspector]
    public EnemyIdleState IdleState;
    [HideInInspector]
    public EnemyBattleState BattleState;

    private EnemyState currentState;

    public EnemyState State
    {
        get => currentState;
        set
        {
            currentState.Exit();
            currentState = value;
            currentState.Enter();
        }
    }

    private void Awake()
    {
        currentState = IdleState = GetComponent<EnemyIdleState>();
        BattleState = GetComponent<EnemyBattleState>();

        currentState.Enter();
    }

    private void Update()
    {
        currentState.StateUpdate();
    }
}