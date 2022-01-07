using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// エディター内で表示するマップの横幅
    /// </summary>
    private const float k_mapWidth = 11.24f;

    [SerializeField] private Transform m_target;

    [SerializeField] private float constraintYMin = 0;
    [SerializeField] private float constraintYMax = Mathf.Infinity;

    [SerializeField] private float offsetY = 3;

    private void LateUpdate()
    {
        TrackCharacter();
    }

    private void TrackCharacter()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Clamp(m_target.position.y + offsetY, constraintYMin, constraintYMax);
        transform.position = pos;
    }

    /// <summary>
    /// エディター内でマップの境界線を表示させる
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(k_mapWidth, int.MaxValue, 0));
    }
}
