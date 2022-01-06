using UnityEngine;

public class PlayerGroundHandler : MonoBehaviour
{
    public bool Grounded { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) Grounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) Grounded = false;
    }
}
