
using UnityEngine;

public class KnockupMushroom : MonoBehaviour
{
    public float launchForce = 20f;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            enemy.OnHit(Vector2.up, launchForce, 1f);
        }
    }
}
