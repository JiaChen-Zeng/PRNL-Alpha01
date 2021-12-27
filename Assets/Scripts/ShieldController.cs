using UnityEngine;

/// <summary>
/// 盾の回転を制御する
/// 盾の回転円の半径は子オブジェクトの Position X で調整してください
/// </summary>
public class ShieldController : MonoBehaviour
{
    [SerializeField] private float m_shieldMoveSpeed = 2f;

    private float rotation
    {
        get
        {
            Vector2 shieldControl = new Vector2(Input.GetAxis("ShieldHorizontal"), Input.GetAxis("ShieldVertical"));
            return Vector2.SignedAngle(Vector2.right, shieldControl);
        }
    }

    private void Start()
    {
        
    }

    void Update()
    {
        transform.localEulerAngles = Vector3.forward * rotation;
    }
}
