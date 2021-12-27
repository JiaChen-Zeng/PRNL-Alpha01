using UnityEngine;

/// <summary>
/// 盾の回転を制御する
/// </summary>
public class ShieldController : MonoBehaviour
{
    [SerializeField] private float m_shieldMoveSpeed = 2f;
    [SerializeField] private float m_shieldClamp = 2f;

    private float m_horizontal = 0f;
    private float m_vertical = 0f;
    private Vector3 m_ClampPosition = new Vector3();
    private Vector3 m_InitialPosition = new Vector3();

    private void Start()
    {
        m_InitialPosition = transform.localPosition;
    }

    void Update()
    {
        m_horizontal = Input.GetAxis("ShieldHorizontal") * Time.deltaTime;
        m_vertical = Input.GetAxis("ShieldVertical") * Time.deltaTime;

        if (m_horizontal == 0 && m_vertical == 0)
            transform.localPosition = m_InitialPosition;
        else
        {
            transform.Translate(m_horizontal * m_shieldMoveSpeed, m_vertical * m_shieldMoveSpeed, 0, Space.Self);
            m_ClampPosition = transform.localPosition - Vector3.zero;
            var magnitude = Vector3.ClampMagnitude(m_ClampPosition, m_shieldClamp);
            transform.localPosition = Vector3.zero + magnitude;
        }
    }
}
