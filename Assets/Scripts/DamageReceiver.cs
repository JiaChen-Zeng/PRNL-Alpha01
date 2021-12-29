using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach this script to all the objects which can be hit by bullet
/// such as Player and shield
/// </summary>
public class DamageReceiver : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogFormat("<color=red>collided with gameobject = {0}</color>", collision.gameObject.name);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            EventManager.pInstance.TriggerDamageReceived(collision.collider);
            EventManager.pInstance.TriggerDestroyBullet(collision.collider);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogFormat("<color=red>trigger with gameobject = {0}</color>", collision.gameObject.name);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            EventManager.pInstance.TriggerDamageReceived(collision);
            EventManager.pInstance.TriggerDestroyBullet(collision);
        }
    }
}
