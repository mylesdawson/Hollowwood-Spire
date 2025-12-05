using UnityEngine;

public class DashEchoLingerer : MonoBehaviour
{
    private Vector3 savedPosition;
    private float destroyTimer = 0.8f;
    ActionContext ctx;

    public void Initialize(ActionContext ctx, Vector3 pos)
    {
        savedPosition = pos;
        this.ctx = ctx;
        ctx.IsDashLocked = true;
    }

    private void Update()
    {
        // Check for dash input even after the dash state ends
        if(ctx.DidDashThisFrame)
        {
            ctx.Transform.position = savedPosition;
            ctx.VelocityY = 0;
            ctx.IsDashLocked = false;
            Destroy(gameObject);
        }

        // Countdown to destruction
        destroyTimer -= Time.deltaTime;
        if(destroyTimer <= 0)
        {
            ctx.IsDashLocked = false;
            Destroy(gameObject);
        }
    }
}