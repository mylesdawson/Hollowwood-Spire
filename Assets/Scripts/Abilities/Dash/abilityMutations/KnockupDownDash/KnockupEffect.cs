using UnityEngine;

public class KnockupEffect : MonoBehaviour
{
    GameObject lKnockup;
    GameObject rKnockup;

    readonly float maxTime = .3f;
    float currentTimer = 0f;
    float targetY;

    public void Initialize(ActionContext ctx, GameObject ground)
    {
        Logger.Log("Knockup Effect Initialized");
        var offset = 4f;
        var plant = Resources.Load<GameObject>("KnockupMushroom");
        var plantSR = plant.GetComponent<SpriteRenderer>();
        float prefabHeight = plantSR.sprite.bounds.size.y * plant.transform.localScale.y;

        targetY = ctx.Transform.position.y;

        var rightPos = new Vector3(ctx.Transform.position.x + offset, ctx.Transform.position.y - prefabHeight, ctx.Transform.position.z);
        var leftPos = new Vector3(ctx.Transform.position.x - offset, ctx.Transform.position.y - prefabHeight, ctx.Transform.position.z);
        lKnockup = Instantiate(plant, rightPos, ctx.Transform.rotation);
        lKnockup.name = "LKnockup";
        rKnockup = Instantiate(plant, leftPos, ctx.Transform.rotation);
        rKnockup.name = "RKnockup";
    }

    void Update()
    {
        if(!lKnockup || !rKnockup)
        {
            return;
        }

        if(currentTimer >= maxTime)
        {
            Destroy(lKnockup);
            Destroy(rKnockup);
            Destroy(this);
        }

        PopUp(lKnockup.transform, currentTimer, maxTime / 3);
        PopUp(rKnockup.transform, currentTimer, maxTime / 3);

        currentTimer += Time.deltaTime;
    }

    void PopUp(Transform t, float time, float duration)
    {
        float start = t.position.y;
        float y = Mathf.Lerp(start, targetY, time / duration);
        t.position = new Vector3(t.position.x, y, t.position.z);
    }

}