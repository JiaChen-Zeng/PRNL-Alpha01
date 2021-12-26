using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private float m_shieldMoveSpeed = 2f;
    [SerializeField] private float m_shieldClamp = 2f;
    private float m_horizontal = 0f;
    private float m_vertical = 0f;
    private Vector3 mClampPosition = new Vector3();
    private Vector3 mInitialPosition = new Vector3();

    private void Start()
    {
        mInitialPosition = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontal = Input.GetAxis("ShieldHorizontal") * Time.deltaTime;
        m_vertical = Input.GetAxis("ShieldVertical") * Time.deltaTime;

        if (m_horizontal == 0 && m_vertical == 0)
            this.transform.localPosition = mInitialPosition;
        else
        {
            this.transform.Translate(m_horizontal * m_shieldMoveSpeed, m_vertical * m_shieldMoveSpeed, 0, Space.Self);
            mClampPosition = this.transform.localPosition - Vector3.zero;
            var magnitude = Vector3.ClampMagnitude(mClampPosition, m_shieldClamp);
            this.transform.localPosition = Vector3.zero + magnitude;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventManager.pInstance.TriggerShieldCollision(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.pInstance.TriggerShieldCollision(collision, false);
    }
}
