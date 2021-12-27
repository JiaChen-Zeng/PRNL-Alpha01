using UnityEngine;

/// <summary>
/// 盾の回転を制御する
/// 盾の回転円の半径は子オブジェクトの Position X で調整してください
/// </summary>
public class ShieldController : MonoBehaviour
{
    [SerializeField] private float m_shieldMoveSpeed = 2f;

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
            return Vector2.SignedAngle(Vector2.right, shieldControl);
        }
    }

    private void Awake()
    {
        m_cc = GetComponentInParent<CharacterController>();
    }

    private void Update()
    {
        transform.localEulerAngles = Vector3.forward * rotation;
    }
}
