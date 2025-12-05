

using UnityEngine;

public class Knockup : MonoBehaviour
{
    GameObject lKnockup;
    GameObject rKnockup;

    public void Initialize(ActionContext ctx, GameObject ground)
    {
        // spawn in two things that will knock up enemy
        var playerPos = ctx.Transform.position;
        var plant = Resources.Load("Plant1_0");
        var offset = 5f;


        SpriteRenderer spriteRenderer = ground.GetComponent<SpriteRenderer>();
        float topYPosition = spriteRenderer.bounds.max.y;

        // Start y pos should be at the ground level...
        var rightPos = new Vector3(ctx.Transform.position.x + offset, ctx.Transform.position.y, ctx.Transform.position.z);
        var leftPos = new Vector3(ctx.Transform.position.y - offset, ctx.Transform.position.y, ctx.Transform.position.z);
        lKnockup = (GameObject)Instantiate(plant, rightPos, ctx.Transform.rotation);
        lKnockup.AddComponent<BoxCollider2D>();
        // lKnockup.GetComponent<SpriteRenderer>().bounds.max.y = topYPosition;
        lKnockup.name = "LKnockup";
        rKnockup = (GameObject)Instantiate(plant, leftPos, ctx.Transform.rotation);
        rKnockup.AddComponent<BoxCollider2D>();
        rKnockup.name = "RKnockup";
    }

    void Awake()
    {

    }

    void Update()
    {
        
    }

}