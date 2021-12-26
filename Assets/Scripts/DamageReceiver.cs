using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script to all the objects which can be hit by bullet
/// such as Player and shield
/// </summary>
public class DamageReceiver : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            EventManager.pInstance.TriggerDamageReceived(other);
            EventManager.pInstance.TriggerDestroyBullet(other);
        }
    }
}
