using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformTracker : MonoBehaviour
{
    private static IEnumerable<Collider2D> GetOverlapPlatforms(Collider2D collider)
    {
        var platforms = new List<Collider2D>();

        var filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Ground"));

        collider.OverlapCollider(filter, platforms);
        return platforms.Where(p => p.CompareTag("Platform"));
    }

    private static GameObject GetRandomPlatformAbove(IEnumerable<GameObject> platforms, Transform transform)
    {
        var platformsAbove = platforms.Where(p => p.transform.position.y > transform.position.y);
        var count = platformsAbove.Count();
        return count == 0 ? null : platformsAbove.ElementAt(Random.Range(0, count));
    }

    private static GameObject GetHighestPlatform(IEnumerable<GameObject> platforms)
    {
        return platforms.OrderByDescending(p => p.transform.position.y).First();
    }

    /// <summary>
    /// 主人公をリードできるプラットフォームを一つ選ぶ
    /// </summary>
    /// <returns>主人公をリードできるようなプラットフォーム</returns>
    public GameObject SelectLeadPlatform(GameObject enemy, Collider2D intrinsicArea)
    {
        var platfromsInArea = GetPlatformsInArea(intrinsicArea);
        return IsLeadingTooFar(enemy)
            ? GetHighestPlatform(platfromsInArea) // 下のプラットフォームに戻るができるだけ高い位置を取る
            : GetRandomPlatformAbove(platfromsInArea, enemy.transform); // TODO: この `enemy.transform` を敵が立っているプラットフォームの `transform` にする。今のままでは立っているプラットフォームも選ばれる。
    }

    private bool IsLeadingTooFar(GameObject gameObject)
    {
        var colliders = new List<Collider2D>();
        GetComponent<Collider2D>().OverlapCollider(new ContactFilter2D(), colliders);
        return !colliders.Contains(gameObject.GetComponent<Collider2D>())
            && transform.position.y < gameObject.transform.position.y;
    }

    private IEnumerable<Collider2D> GetPlatforms()
    {
        return GetOverlapPlatforms(GetComponent<Collider2D>());
    }

    private IEnumerable<GameObject> GetPlatformsInArea(Collider2D intrinsicArea)
    {
        return GetPlatforms().Intersect(GetOverlapPlatforms(intrinsicArea)).Select(p => p.gameObject);
    }
}
