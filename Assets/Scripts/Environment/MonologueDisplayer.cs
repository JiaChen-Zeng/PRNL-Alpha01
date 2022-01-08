using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonologueDisplayer : MonoBehaviour
{
    bool isCalledOnce = false;

    [SerializeField] string Monologue = "";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCalledOnce)
        {
            isCalledOnce = true;
            Debug.Log(Monologue);
        }
    }
}