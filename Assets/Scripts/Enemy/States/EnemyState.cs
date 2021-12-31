using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    protected EnemyAI ai;

    protected virtual void Awake()
    {
        ai = GetComponent<EnemyAI>();
    }

    public virtual void Enter() { }
    public virtual void StateUpdate() { }
    public virtual void Exit() { }
}
