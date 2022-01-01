using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    public EnemyAI Ai { get; private set; }

    protected virtual void Awake()
    {
        Ai = GetComponent<EnemyAI>();
    }

    public virtual void Enter() { }
    public virtual void StateUpdate() { }
    public virtual void Exit() { }
}
