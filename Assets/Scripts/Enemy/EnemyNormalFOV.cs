using UnityEngine;

/// <summary>
/// 雑魚敵の視野は円の半径がある
/// </summary>
public class EnemyNormalFOV : EnemyFOV
{
    public float Radius
    { 
        get => GetComponent<CircleCollider2D>().radius;
        internal set { GetComponent<CircleCollider2D>().radius = value; }
    }
}
