using System;
using UnityEngine;

/// <summary>
/// 盾の回転を制御する
/// 盾の回転円の半径は子オブジェクトの Position X で調整してください
/// </summary>
public class ShieldController : MonoBehaviour
{
    [SerializeField] private float m_rotateSpeed = 0.1f;

    /// <summary>
    /// プレイヤーが盾の制御を行っているか。true は行っている。
    /// </summary>
    public bool IsControlling
    {
        get => m_control != Vector2.zero;
    }

    /// <summary>
    /// 盾実際にいる回転位置 `localEulerAngles`（盾の最終 tween 位置 `m_rotation` ではない）が前向きか
    /// </summary>
    public bool FacingFront
    {
        get => Math.Abs(m_destinationAngle) <= 90;
    }

    /// <summary>
    /// プレイヤーの盾位置調整の入力
    /// </summary>
    private Vector2 m_control
    {
        get => new Vector2(Input.GetAxis("ShieldHorizontal"), Input.GetAxis("ShieldVertical"));
    }

    /// <summary>
    /// 盾の回転角度
    /// </summary>
    private float m_rotation
    {
        get
        {
            float angle = Vector2.SignedAngle(m_cc.FacingRight ? Vector2.right : Vector2.left, m_control);
            if (!m_cc.FacingRight) angle = -angle;
            return angle;
        }
    }

    /// <summary>
    /// 主人公の向きを取得するため。盾の回転位置は向きによるからだ。
    /// </summary>
    private CharacterController m_cc;

    /// <summary>
    /// 範囲が [0, 360) の `localEulerAngles` を (-180, 180] に落とした回転角
    /// </summary>
    private float m_destinationAngle
    {
        get
        {
            float angle = transform.localEulerAngles.z;
            return (angle > 180) ? angle - 360 : angle;
        }
    }

    private void Awake()
    {
        m_cc = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        Rotate();
    }
    
    /// <summary>
    /// 主人公が盾の位置調整によって反転するとき、盾も一緒に反転してみえるように回転角度を調整する
    /// </summary>
    public void Flip()
    {
        // 原型：Vector3.forward * (90 - (m_destinationAngle - 90))。左向きの場合は一部の数字がマイナスになる
        transform.localEulerAngles = Vector3.forward * ((m_cc.FacingRight ? 1 : -1) * 180 - m_destinationAngle);
    }

    /// <summary>
    /// プレイヤー入力によって盾を回転する
    /// </summary>
    private void Rotate()
    {
        float dRot = m_rotation - m_destinationAngle;
        if (180 < Mathf.Abs(dRot)) // 普通の回転方向ではなく逆に回したほうが最短距離の場合、つまり変化量が 180 より大きい場合
        {
            // 今の回転方向に応じて実際の回転方向が逆になるように調整
            if (0 < dRot) dRot -= 360;
            else dRot += 360;
        }

        transform.localEulerAngles = Vector3.forward * (m_destinationAngle + dRot * m_rotateSpeed);
    }
}
