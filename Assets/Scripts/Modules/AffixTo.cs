using UnityEngine;

public enum AffixTimingOptions
{
	never, // disable the affixing feature
	onAspectChange, // onload and when screen (or relative bounds) aspect ratio changes
	everyFrame //use if relative bounds could move at other times then on aspect change
}

public class AffixTo : MonoBehaviour
{
    public bool loadOffScreen;
    // --- affix features -- note: affix features dont account for rotated vector spaces if this is required dont use this feature
	public Vector2 affixToPos; // world space padding (relative to button center) if you want to include this buttons size in the padding use affixToPosAddSelfSize (you can use both if you want size relative plus an extra amount if padding)
	public Vector2 affixToPosAddSelfExtent; // add a number of x and/or y bound extents to affixToPos (example : (1,1) would move button right and up equal to the button extents (basically centering it to the bottom left))
	public Vector2 affixToAnchorPoint; // affix button to percent of sizes of relative affix object. if affixRelativeTo (or doesnt have bounds) button will affix to screen (example: (1,-.5) button would affix to right edge of affixRelativeTo or screen and 1/4 of the way from the bottom of the container)
	public Vector2 affixScale; //scale match % of what this button is affixed to.  if an value is 0 then that axis scale is not changed (feature disabled for that axis). if a value is negative then it will act as though it is positive except it will scale relative to the other axis (useful if you want to scale but maintain aspect ratio of contents).

	//leave this null (or doesnt have bounds) but change affixToTiming to affix to screen
	//if this isnt null and has a spriterenderer or collider instead of affixing to screen it will affix relative to this object instead of the screen
	public GameObject affixRelativeTo;  
	public AffixTimingOptions affixToTiming;// when to recheck affix features. use this to disable affix features.
	//---

    public SpriteRenderer shape;

    private Vector2 lastAffixPos;

	private Vector3 initialPos;
    private float lastScreenSize = 0.0f;
	private float lastAspect = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastAffixPos = new Vector2(0,0);
        initialPos = transform.localPosition;
		if (loadOffScreen && affixToTiming != AffixTimingOptions.never)
		{
			transform.position = new Vector3(1000f, 1000f, transform.position[2]);
		}
    }

    // Update is called once per frame
    void Update()
    {
        updateAffix(false);
    }

    public void updateAffix(bool overrideTiming = true)
	{
		if(affixToTiming != AffixTimingOptions.never || overrideTiming)
		{
			var hasRelativeBounds = false;
			var boundingVector = Vector2.zero;
			if(affixRelativeTo != null)
			{
				if(affixRelativeTo.GetComponent<Collider2D>() != null)
				{
					boundingVector = new Vector2(affixRelativeTo.GetComponent<Collider2D>().bounds.size.x,affixRelativeTo.GetComponent<Collider2D>().bounds.size.y);
					hasRelativeBounds = true;
				}

				if(affixRelativeTo.GetComponent<PhysicsFreeCollider>() != null)
				{
					boundingVector = affixRelativeTo.GetComponent<PhysicsFreeCollider>().getExtents() * 2f;
					hasRelativeBounds = true;
				}

				if(affixRelativeTo.GetComponent<SpriteRenderer>() != null)
				{
					boundingVector = new Vector2(affixRelativeTo.GetComponent<SpriteRenderer>().bounds.size.x,affixRelativeTo.GetComponent<SpriteRenderer>().bounds.size.y);
					hasRelativeBounds = true;
				}
			}
			var currentOrthoSize = Camera.main.orthographicSize;
			var currentAspect = Camera.main.aspect;
			var currentAffixPos = Vector2.zero;
			if(hasRelativeBounds)
			{// not relative to screen so check if relative bound's aspect changes instead
				currentOrthoSize = boundingVector[0]; 
				currentAspect = boundingVector[1];
				currentAffixPos = new Vector2(affixRelativeTo.transform.position[0],affixRelativeTo.transform.position[1]);
			}
			if((((lastScreenSize != currentOrthoSize || lastAspect != currentAspect || (hasRelativeBounds && lastAffixPos != currentAffixPos)) && affixToTiming == AffixTimingOptions.onAspectChange) || affixToTiming == AffixTimingOptions.everyFrame)||overrideTiming)
			{
				var height = 2f * currentOrthoSize;
				var width = height * currentAspect;
				
				var relativeScaleVector = boundingVector;
				if (!hasRelativeBounds)
				{
					relativeScaleVector = new Vector3(width, height, 1f);
				}
				var xScaleAxis = ((affixScale[0] < 0f) ? 1 : 0);
				var yScaleAxis = ((affixScale[1] < 0f) ? 0 : 1);
				if(affixScale[0] != 0f && affixScale[1] != 0f)
				{
					transform.localScale = new Vector3(1,1,1);
					transform.localScale = new Vector3(Mathf.Abs(affixScale[0])*relativeScaleVector[xScaleAxis]/shape.bounds.size[xScaleAxis],Mathf.Abs(affixScale[1])*relativeScaleVector[yScaleAxis]/shape.bounds.size[yScaleAxis],1f);
				}
				else
				{
					if(affixScale[0] != 0f)
					{
						transform.localScale = new Vector3(1,transform.localScale[1],1);
						transform.localScale = new Vector3(Mathf.Abs(affixScale[0])*relativeScaleVector[xScaleAxis]/shape.bounds.size[xScaleAxis],transform.localScale[1],1f);
					}
					if(affixScale[1] != 0f)
					{
						transform.localScale = new Vector3(transform.localScale[0],1,1);
						transform.localScale = new Vector3(transform.localScale[0],Mathf.Abs(affixScale[1])*relativeScaleVector[yScaleAxis]/shape.bounds.size[yScaleAxis],1f);
					}

				}

				if(hasRelativeBounds)
				{
					transform.position = affixRelativeTo.transform.position +  new Vector3((affixToAnchorPoint[0]*boundingVector[0]/2f) + affixToPos[0] + (affixToPosAddSelfExtent[0] * shape.bounds.extents.x),(affixToAnchorPoint[1]*boundingVector[1]/2f) + affixToPos[1] + (affixToPosAddSelfExtent[1] * shape.bounds.extents.y),initialPos[2]-1f);
				}
				else
				{
					transform.position = new Vector3((affixToAnchorPoint[0]*width/2f) + affixToPos[0] + (affixToPosAddSelfExtent[0] * shape.bounds.extents.x),(affixToAnchorPoint[1]*height/2f) + affixToPos[1] + (affixToPosAddSelfExtent[1] * shape.bounds.extents.y),transform.position[2]);
				}

				lastScreenSize = currentOrthoSize;
				lastAspect = currentAspect;
			}
		}
	}
}
