using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 盾の回転を制御する
/// 盾の回転円の半径は子オブジェクトの Position X で調整してください
/// </summary>
public class ShieldController : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 0.1f;

    /// <summary>
    /// プレイヤーが盾の制御を行っているか。true は行っている。
    /// </summary>
    public bool IsControlling { get => Control != Vector2.zero; }

    /// <summary>
    /// 盾実際にいる回転位置（盾の最終 tween 位置 `m_rotation` ではない）が前向きか
    /// </summary>
    public bool FacingFront { get => Math.Abs(DestinationAngle) <= 90; }

    /// <summary>
    /// 盾実際にいる回転位置が下を向いているか。主に盾ジャンプの判定に使う
    /// </summary>
    public bool PointingDownwards
    {
        get
        {
            var angle = DestinationAngle;
            return -150 < angle && angle < -30;
        }
    }

    public bool CollidedWithGround
    {
        get
        {
            var groundFilter = new ContactFilter2D();
            groundFilter.SetLayerMask(LayerMask.GetMask("Ground"));

            var colliders = new List<Collider2D>();
            shieldCollider.OverlapCollider(groundFilter, colliders);
            return 0 < colliders.Count;
        }
    }

    /// <summary>
    /// プレイヤーの盾位置調整の入力
    /// </summary>
    private Vector2 Control
    {
        get => characterController.Controllable
            ? new Vector2(Input.GetAxis("ShieldHorizontal"), Input.GetAxis("ShieldVertical"))
            : Vector2.zero;
    }

    /// <summary>
    /// 盾の回転角度
    /// </summary>
    private float Rotation
    {
        get
        {
            float angle = Vector2.SignedAngle(characterController.FacingRight ? Vector2.right : Vector2.left, Control);
            if (!characterController.FacingRight) angle = -angle;
            return angle;
        }
    }

    /// <summary>
    /// 主人公の向きを取得するため。盾の回転位置は向きによるからだ。
    /// </summary>
    private CharacterController characterController;
    /// <summary>
    /// 盾ジャンプの判定をするため
    /// </summary>
    private Collider2D shieldCollider;

    /// <summary>
    /// 範囲が [0, 360) の `localEulerAngles` を (-180, 180] に落とした回転角
    /// </summary>
    private float DestinationAngle
    {
        get
        {
            float angle = transform.localEulerAngles.z;
            return (angle > 180) ? angle - 360 : angle;
        }
    }

    private void Awake()
    {
        characterController = GetComponentInParent<CharacterController>();
        shieldCollider = GetComponentInChildren<Collider2D>();
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
        transform.localEulerAngles = Vector3.forward * ((characterController.FacingRight ? 1 : -1) * 180 - DestinationAngle);
    }

    /// <summary>
    /// プレイヤー入力によって盾を回転する
    /// </summary>
    private void Rotate()
    {
        float dRot = Rotation - DestinationAngle;
        if (180 < Mathf.Abs(dRot)) // 普通の回転方向ではなく逆に回したほうが最短距離の場合、つまり変化量が 180 より大きい場合
        {
            // 今の回転方向に応じて実際の回転方向が逆になるように調整
            if (0 < dRot) dRot -= 360;
            else dRot += 360;
        }

        transform.localEulerAngles = Vector3.forward * (DestinationAngle + dRot * rotateSpeed);
    }
}
