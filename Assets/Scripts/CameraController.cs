using UnityEngine;

public class CameraController : MonoBehaviour
{
    /// <summary>
    /// エディター内で表示するマップの横幅
    /// </summary>
    private const float k_mapWidth = 11.24f;

    [SerializeField] private float m_constrainYMin = 0;
    [SerializeField] private float m_constrainYMax = Mathf.Infinity;
    [SerializeField] private Transform m_target;
    private Vector3 mNewPosition = new Vector3();
  
    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void LateUpdate()
    {
        DistanceCheck();
        Follow();
    }

    private void Follow()
    {
        if(m_target.position.y > this.transform.position.y)
        {
            mNewPosition.x = this.transform.position.x;
            mNewPosition.y = m_target.position.y;
            mNewPosition.z = this.transform.position.z;
            this.transform.position = mNewPosition;
            SetNewPosition(this.transform.position.x, m_target.position.y, this.transform.position.z);
        }
    }

    private void SetNewPosition(float x, float y, float z)
    {
        mNewPosition.x = x;
        mNewPosition.y = Mathf.Clamp(y, m_constrainYMin, m_constrainYMax);
        mNewPosition.z = z;
        this.transform.position = mNewPosition;
    }

    private void DistanceCheck()
    {
        var direction = m_target.position - this.transform.position;
        var dot = Vector3.Dot(m_target.up, direction.normalized);
        if (dot < 0)
        {
            SetNewPosition(this.transform.position.x, m_target.position.y, this.transform.position.z);
        }
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
