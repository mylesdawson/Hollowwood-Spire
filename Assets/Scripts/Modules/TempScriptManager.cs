using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Unity.VisualScripting;

public enum TempScriptTag
{
    addOn, //if the script doesnt need a reference object it will reference its host and should have this tag (scripts that reference an object that is not created in the rebuild script should have the addOn tag)
}

public enum TempScriptRemovalType
{
    ManagerDestroyed,
    ReferenceObjectMissing
}

public class TempScript
{
	public Transform referenceObject; //the GameObject that this script references (the object returned by its build script) (scripts that reference an object that is not created in the rebuild script should have the addOn tag)
	public Action<TempScript> updateScript; //called each frame (unless updateSchedule is not 1 (usually is 1))
	public Func<TempScript,Transform> buildScript; //this is similar to Awake. It is the method of creating the reference object. If no reference object is needed this script should have the addOn tag and reference its host. If this script doesnt create an object but instead finds an existing object that was already created it should also have the addOn tag.
	public List<TempScript> animationStorage; // save other TempScripts for later. used for things like replacing old animations after this one is done.
	public TempScriptManager createdFromRef; // reference to the manager of this script

	public bool isFirst; //is it currently the first frame that this script is running
    public float timer; //the amount of time the script has existed for (not dependent on updateSchedule)
    public List<Action<TempScriptRemovalType>> onRemovalRequests; //all are called when either the host is destroyed or if the reference object is destroyed or null
	
	public int updateSchedule; //call script every this number of frames auto set to 1. setting to 0 makes this only called when called directly
	public int updateIndex; // number of frames since last update

    public List<string> dynamicTags;
    public List<TempScriptTag> staticTags;

    public bool hasTag(TempScriptTag tempScriptTag)
    {
        return staticTags.Contains(tempScriptTag) || dynamicTags.Contains(tempScriptTag.ToString());
    }

    public bool hasTag(string tempScriptTag)
    {
        return dynamicTags.Contains(tempScriptTag) || staticTags.Contains((TempScriptTag) Enum.Parse(typeof(TempScriptTag), tempScriptTag, true));
    }

	public TempScript() 
    {
        updateIndex = 0;
        updateSchedule = 1;
        updateScript = null;
        animationStorage = new List<TempScript>();
        isFirst = true;
        dynamicTags = new List<string>();
        staticTags = new List<TempScriptTag>();
        onRemovalRequests = new List<Action<TempScriptRemovalType>>();
    }
}

public class TempScriptManager : MonoBehaviour
{
	public GameObject host; //ideally this should be the parent to this object. should be mapped to the object in inspector. 
	
	public TempScriptDictionary tempScriptDictionary; //a place to put functions that create tempScripts or partial tempScripts so that they can be used in multiple places to avoid code duplication.

	public List<Func<TempScript,Action<TempScript>>> addAdditionalScriptsWithCondition; // all functions in this list will be called when a new animation script is added and any that dont return null will be added to the newly added script (note: use addToAdditionalScriptsWithCondition to add more (if you are adding one at runtime) so that this list isnt overwritten)

	private Stack<TempScript> callStack; // not sure if we need this just dont what to risk over filling the real call stack if there are a huge amount of temp scripts so this is just a fake in memory call stack 
	private List<TempScript> tempScripts;
	
	void Awake()
	{
		tempScripts = new List<TempScript>();
		callStack = new Stack<TempScript>();
		if(addAdditionalScriptsWithCondition == null)
		{
			addAdditionalScriptsWithCondition = new List<Func<TempScript,Action<TempScript>>>();
		}
	}

    // Update is called once per frame
    void Update()
    {
        var scriptCount = tempScripts.Count;
		foreach(var TempScript in tempScripts)
		{
			callStack.Push(TempScript);
		}

		while(callStack.Count > 0)
		{
			var scriptToRun  = callStack.Pop();
            scriptToRun.timer = scriptToRun.timer + Time.deltaTime;
			if(scriptToRun.referenceObject != null)
			{
				if(scriptToRun.updateScript != null)
				{
					scriptToRun.updateIndex = scriptToRun.updateIndex + 1;
					if(scriptToRun.updateIndex == scriptToRun.updateSchedule)
					{
						scriptToRun.updateScript(scriptToRun);
						scriptToRun.updateIndex = 0;
						scriptToRun.isFirst = false;
					}
				}
			}
			else
			{
                foreach(var removeRequest in scriptToRun.onRemovalRequests)
                {
                    if(removeRequest != null)
                    {
                        removeRequest(TempScriptRemovalType.ReferenceObjectMissing);
                    }
                }
				tempScripts.Remove(scriptToRun);
			}
		}
    }


	//returns a buildScript for adding a gameObject (if path points to prefab it creates a script to load it. if points to sprite it makes a new object and adds a spriteRenderer to display the sprite) (note: opacity only does anything if the resulting object has a spriteRenderer)
	//get a preset build script that can be used for adding animations that dont require: 
	// -- the animation to change before its first update and needs to be able to be rebuilt by another out of starting (where it is added from) scope script
	// -- requires its local scope to be rebuilt by script that doesnt share a starting scope (for this one you can use this but will need to add to the rebuild function later unless the original script also requires a new addition to it's local scope then either add script that adds more scripts to keep it local or make the rebuild scripts from scratch)
	// -- the animation not a prefab or sprite
	public static Func<TempScript,Transform> getBasicBuildScript(string path, bool asChild = true, float opacity = 1f, float relativeDepth = -.2f, Action<TempScript> extendBuildScript = null)
	{
			return (anSc)=>{
				GameObject animationTemplate = (GameObject)Resources.Load(path, typeof(GameObject));
				Transform an = null;
				if (animationTemplate == null)
				{
					an = new GameObject("autoAddedArtHolder").transform;
					if (asChild)
					{
						an.SetParent(anSc.createdFromRef.host.transform, false);
					}
					if (path != "")
					{
						an.AddComponent<SpriteRenderer>().sprite = (Sprite)Resources.Load(path, typeof(Sprite));
					}
					else
					{
						an.AddComponent<SpriteRenderer>();
					}
				}
				else
				{
					if (asChild && anSc.createdFromRef.host != null)
					{
						an = Instantiate(animationTemplate, anSc.createdFromRef.host.transform).transform;
					}
					else
					{
						an = Instantiate(animationTemplate).transform;
						an.localScale = anSc.createdFromRef.host.transform.localScale;
					}
				}
				if(an.GetComponent<SpriteRenderer>()!=null)
				{
					var newColor = an.GetComponent<SpriteRenderer>().color;
					newColor[3] = opacity;
					an.GetComponent<SpriteRenderer>().color = newColor;
				} 
				an.position = anSc.createdFromRef.host.transform.position + new Vector3(0,0,relativeDepth);
                if(extendBuildScript != null)
                {
                    anSc.referenceObject = an;
                    extendBuildScript(anSc);
                }
				return an;
			};
	}
	
    //Create a TempScript and add it to this Manager (note: the buildScript will be run immediately)
    //if build script is null it instead points reference object to host and adds the addOn tag to this script
    //the update script of the created temp script will not be over written it will add it to any existing script it might have obtained from the build script (null makes no change to the updateScript)
	public TempScript addTempScript(Func<TempScript,Transform> buildScript = null, Action<TempScript> updateScript = null, List<TempScriptTag> staticTags = null, List<string> dynamicTags = null)
	{
		var tempScript = new TempScript();
        if(staticTags != null)
        {
            tempScript.staticTags.AddRange(staticTags); 
        }
        if(dynamicTags != null)
        {
            tempScript.dynamicTags.AddRange(dynamicTags); 
        }
        if(buildScript == null)
        {
            tempScript.staticTags.Add(TempScriptTag.addOn);
            tempScript.buildScript = (tS)=>{return tS.createdFromRef.host.transform;};
        }
        else
        {
            tempScript.buildScript = buildScript;
        }
		tempScript.createdFromRef = transform.GetComponent<TempScriptManager>();
		var an = tempScript.buildScript(tempScript);
		tempScript.referenceObject = an;
        if(updateScript != null)
        {
            if(tempScript.updateScript == null)
            {
                tempScript.updateScript = updateScript;
            }
            else
            {
                addToExistingScript(tempScript, updateScript);
            }
        }
		

		foreach (var possibleScriptAddition in addAdditionalScriptsWithCondition)
		{
			var scriptAddition = possibleScriptAddition(tempScript);
			if(scriptAddition != null)
			{
				addToExistingScript(tempScript,scriptAddition);
			}
		}
		tempScripts.Add(tempScript);
		return tempScript;
	}

    //just helps with adding scripts together (if you want to add extra code to the script)
	public static void addToExistingScript(TempScript scriptToAddTo, Action<TempScript> scriptToAdd = null, Action<TempScript> additionalRebuildScript = null)
	{
		if(additionalRebuildScript != null)
		{
			Func<Func<TempScript,Transform>,Action<TempScript>,Func<TempScript,Transform>> rebuildAppender = (aS,s)=>{
				return (anSc)=>{
					s(anSc);
					return aS(anSc);
				};
			};
			additionalRebuildScript(scriptToAddTo);
			scriptToAddTo.buildScript = rebuildAppender(scriptToAddTo.buildScript,additionalRebuildScript);
		}
		if(scriptToAdd!=null)
		{
			if(scriptToAddTo.updateScript == null)
			{
				scriptToAddTo.updateScript = scriptToAdd;
			}
			else
			{
				Func<Action<TempScript>,Action<TempScript>,Action<TempScript>> scriptAppender = (aS,s)=>{
					return (anSc)=>{
						aS(anSc);
						s(anSc);
					};
				};
				scriptToAddTo.updateScript = scriptAppender(scriptToAddTo.updateScript,scriptToAdd);
			}
		}
	}
	
    //also returns all the tempScripts that were removed in case you want to add them to storage or something
	public List<TempScript> removeTempScriptByTag(string tag, bool skipDestroy = false)
	{
		var rl = tempScripts.Where(x=>x.hasTag(tag)).ToList();
		//TempScripts = TempScripts.Where(x=>x.data != "base").ToList();
		foreach(var r in rl)
		{
			if(skipDestroy)
			{
				r.referenceObject = null;
			}
			else
			{
				if(r.hasTag(TempScriptTag.addOn))
				{
					r.referenceObject = null;
				}
				else
				{
					if(r.referenceObject!=null)
					{
						Destroy(r.referenceObject.gameObject);
					}
				}
			}
		}
		return rl;
	}
	
    //just sets them all to be removed and destroys any reference objects so the tempScripts wont be fully removed until next frame
	public void removeAllTempScripts()
	{
		foreach(var anSc in tempScripts)
		{
			if(anSc.referenceObject != null)
			{
				if(anSc.hasTag(TempScriptTag.addOn))
				{
					anSc.referenceObject = null;
				}
				else
				{
					Destroy(anSc.referenceObject.gameObject);
				}
			}
		}
	}
	
	public List<TempScript> getTempScriptsByTag(string tag)
	{
		return tempScripts.Where(x=>x.hasTag(tag)).ToList();
	}

    public List<TempScript> getTempScriptsByTag(TempScriptTag tag)
	{
		return tempScripts.Where(x=>x.hasTag(tag)).ToList();
	}

	public List<Transform> getReferenceObjectsByTag(string tag)
	{
		var l = getTempScriptsByTag(tag);
		var rl = new List<Transform>();
		foreach(var r in l)
		{
			rl.Add(r.referenceObject);
		}
		return rl;
	}

    public List<Transform> getReferenceObjectsByTag(TempScriptTag tag)
	{
		var l = getTempScriptsByTag(tag);
		var rl = new List<Transform>();
		foreach(var r in l)
		{
			rl.Add(r.referenceObject);
		}
		return rl;
	}

	public void addToAdditionalScriptsWithCondition(Func<TempScript,Action<TempScript>> conditionalScriptToAdd)
	{
		if(addAdditionalScriptsWithCondition == null)
		{
			addAdditionalScriptsWithCondition = new List<Func<TempScript,Action<TempScript>>>();
		}
		addAdditionalScriptsWithCondition.Add(conditionalScriptToAdd);
	}

	void OnDestroy()
	{
        foreach (var tempScript in tempScripts)
        {
            foreach(var removeRequest in tempScript.onRemovalRequests)
            {
                if(removeRequest != null)
                {
                    removeRequest(TempScriptRemovalType.ManagerDestroyed);
                }
            }
        }
	}
}