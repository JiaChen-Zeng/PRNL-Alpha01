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

    private void OnDestroy()
    {
        EventManager.pInstance.OnDamageReceived -= OnDamageReceived;
    }
}
