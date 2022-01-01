using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformTracker : MonoBehaviour
{
    private HashSet<GameObject> platforms = new HashSet<GameObject>();

    public bool HasPlatform { get => platforms.Count != 0; }

    public GameObject GetRandomPlatform()
    {
        return platforms.ElementAt(Random.Range(0, platforms.Count));
    }

    private void OnTriggerEnter2D(Collider2D platform)
    {
        if (!platform.CompareTag("Platform")) return;

        platforms.Add(platform.gameObject);
    }

    private void OnTriggerExit2D(Collider2D platform)
    {
        if (!platform.CompareTag("Platform")) return;

        platforms.Remove(platform.gameObject);
    }
}
