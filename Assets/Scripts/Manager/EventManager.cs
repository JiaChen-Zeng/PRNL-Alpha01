using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    private static EventManager mInstance = null;
    public static EventManager pInstance
    {
        get
        {
            if(mInstance == null)
                mInstance = UnityEngine.Object.FindObjectOfType<EventManager>();
            return mInstance;
        }
    }

    public delegate void ShieldCollision(Collider2D collider, bool enter);
    public event ShieldCollision OnShieldCollision;

    public void TriggerShieldCollision(Collider2D collider, bool enter)
    {
        if (OnShieldCollision != null)
            OnShieldCollision.Invoke(collider, enter);
    }

    public delegate void DamageReceived(Collider2D bullet);
    public event DamageReceived OnDamageReceived;

    public void TriggerDamageReceived(Collider2D bullet)
    {
        if (OnDamageReceived != null)
            OnDamageReceived.Invoke(bullet);
    }

    public delegate void DestroyBullet(Collider2D bullet);
    public event DestroyBullet OnDestroyBullet;

    public void TriggerDestroyBullet(Collider2D bullet)
    {
        if (OnDestroyBullet != null)
            OnDestroyBullet.Invoke(bullet);
    }
}
