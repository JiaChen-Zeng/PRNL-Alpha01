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
}
