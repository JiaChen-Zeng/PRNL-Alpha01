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
    /// 主人公の向きを取得するため。盾の回転位置は向きによるからだ。
    /// </summary>
    private CharacterController m_cc;

    /// <summary>
    /// 盾の回転角度
    /// </summary>
    private float rotation
    {
        get
        {
            float x = (m_cc.FacingRight ? 1 : -1) * Input.GetAxis("ShieldHorizontal");
            float y = Input.GetAxis("ShieldVertical");
            Vector2 shieldControl = new Vector2(x, y);
            float angle = Vector2.SignedAngle(Vector2.right, shieldControl);
            if (angle < 0) angle += 360;
            return angle;
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
    /// プレイヤー入力によって盾を回転する
    /// </summary>
    private void Rotate()
    {
        float dRot = rotation - transform.localEulerAngles.z;
        if (180 < Mathf.Abs(dRot)) // 普通の回転方向ではなく逆に回したほうが最短距離の場合、つまり変化量が 180 より大きい場合
        {
            // 今の回転方向に応じて実際の回転方向が逆になるように調整
            if (0 < dRot) dRot -= 360;
            else dRot += 360;
        }
        transform.localEulerAngles = Vector3.forward * (transform.localEulerAngles.z + dRot * m_rotateSpeed);
    }
}
