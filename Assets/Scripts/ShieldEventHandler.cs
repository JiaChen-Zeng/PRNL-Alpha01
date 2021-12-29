using UnityEngine;

public class ShieldEventHandler : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float m_shieldCapacity;

    private void OnEnable()
    {
        EventManager.pInstance.OnDamageReceived += OnDamageReceived;
    }

    private void OnDamageReceived(Collider2D bullet)
    {
        //substract the shield capacity and rest reduce the hitpoints
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
