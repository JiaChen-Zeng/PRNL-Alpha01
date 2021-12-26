using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    [SerializeField] private float m_shieldMoveSpeed = 2f;

    [Header("Stats")]
    [SerializeField] private float m_shieldCapacity;

    private float m_horizontal = 0f;
    private float m_vertical = 0f;
    private Vector3 mClampPosition = new Vector3();

    private void OnEnable()
    {
        EventManager.pInstance.OnDamageReceived += OnDamageReceived;
    }

    private void OnDamageReceived(Collider bullet)
    {
        //substract the shield capacity and rest reduce the hitpoints
    }

    // Update is called once per frame
    void Update()
    {
        m_horizontal = Input.GetAxis("ShieldHorizontal") * Time.deltaTime;
        m_vertical = Input.GetAxis("ShieldVertical") * Time.deltaTime;

        

        //Vector2 translate = new Vector2()
        this.transform.Translate(m_horizontal * m_shieldMoveSpeed, m_vertical * m_shieldMoveSpeed, 0, Space.Self);
        mClampPosition.x = Mathf.Clamp(this.transform.localPosition.x, -1, 1);
        mClampPosition.y = Mathf.Clamp(this.transform.localPosition.y, -1, 1);
        mClampPosition.z = 0;
        this.transform.localPosition = mClampPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EventManager.pInstance.TriggerShieldCollision(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        EventManager.pInstance.TriggerShieldCollision(collision, false);
    }

    private void OnDestroy()
    {
        EventManager.pInstance.OnDamageReceived -= OnDamageReceived;
    }
}
