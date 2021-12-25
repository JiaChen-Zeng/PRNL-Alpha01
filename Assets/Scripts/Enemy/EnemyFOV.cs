using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 敵の視野に入ったかどうかを事件で知らせる
/// </summary>
public class EnemyFOV : MonoBehaviour
{
    public readonly UnityEvent<GameObject> OnEnterFOV = new UnityEvent<GameObject>();
    public readonly UnityEvent<GameObject> OnExitFOV = new UnityEvent<GameObject>();

    public float FOV
    { 
        get => GetComponent<CircleCollider2D>().radius;
        internal set { GetComponent<CircleCollider2D>().radius = value; }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        OnEnterFOV.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        OnExitFOV.Invoke(collision.gameObject);
    }
}
