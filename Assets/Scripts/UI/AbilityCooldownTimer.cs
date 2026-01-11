
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



public class AbilityCooldownTimer : MonoBehaviour
{
    
    public GameObject art;
    public GameObject cooldownIndicator;

    private float startTime;
    private List<Vector2> currentShape;

    private Vector3[] currentVertexMap;
    private Vector2[] currentUVMap;
    private int[] currentTriangleMap;
    private int startIndex;
    private float cooldownTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentShape = new List<Vector2>();
        setAbilityArt(art.GetComponent<SpriteRenderer>().sprite);
        startTimer(15f);
    }

    // Update is called once per frame
    void Update()
    {
        if(startTime <= 0f){return;}

        if(cooldownTime < 0f)
        {
            cooldownIndicator.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            cooldownTime = cooldownTime - Time.deltaTime;
            updateIndicator();
            cooldownIndicator.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void setAbilityArt(Sprite sprite) // only works for convex shapes and the art must only contain one shape
    {
        cooldownIndicator.GetComponent<MeshRenderer>().enabled = false;
        art.GetComponent<SpriteRenderer>().sprite = sprite;
        transform.GetComponent<PhysicsFreeCollider>().setHitBox(art.transform);
        setupMesh();
    }


    public void startTimer(float amountOfTime)
    {
        cooldownTime = amountOfTime;
        startTime = amountOfTime;
        setupMesh();
    }

    private void setupMesh()
    {
        var pfc = transform.GetComponent<PhysicsFreeCollider>();
        var lineLength = pfc.getExtents().magnitude;
        var center = pfc.getCenter();
        var endLine = new Vector2(0f, lineLength) + center;
        var intersection = pfc.getLineIntersections((center, endLine))[0];
        startIndex = intersection.Item2.Item1.Item2;
        if(startIndex > 0)
        {
            startIndex = startIndex - 1;
        }
        
        currentShape.Clear();
        
        for(int i = 0;i<pfc.shapeList[0].Count;i++)
        {
            currentShape.Add(pfc.shapeList[0][(i+startIndex)%pfc.shapeList[0].Count]);
        }
        currentShape.Reverse();
        
        currentShape.Add(intersection.Item1);
        cooldownIndicator.GetComponent<MeshFilter>().mesh = MeshBuilderUtility.meshFromTwoShapes(currentShape, new List<Vector2>{transform.GetComponent<PhysicsFreeCollider>().getLocalCenter()});
        currentShape.Insert(0, intersection.Item1);
        currentVertexMap = cooldownIndicator.GetComponent<MeshFilter>().mesh.vertices;
        currentUVMap = cooldownIndicator.GetComponent<MeshFilter>().mesh.uv;
        currentTriangleMap = cooldownIndicator.GetComponent<MeshFilter>().mesh.triangles;

    }

    private void updateIndicator()
    {
        var pfc = transform.GetComponent<PhysicsFreeCollider>();
        var lineLength = pfc.getLocalExtents().magnitude;
        var localCenter = pfc.getLocalCenter();

        var currentLine = VectorSpaceConversionUtility.vector3ToVector2(Quaternion.Euler(0f,0f,cooldownTime*360f/startTime) * (new Vector3(0f, lineLength,0f))) + localCenter;

        if(currentShape.Count<3){return;}

        var foundCollisionPoint = Vector2.zero;
        var vertexToClear = 0;
        var lastVert = currentShape[0];
        var currentVert =  currentShape[1];
        for(int i = 0;i<currentShape.Count;++i)
        {
            var intersection = HitBoxUtility.getIntersection((localCenter, currentLine), (lastVert, currentVert));
            // Debug.DrawLine(localCenter, currentLine, Color.green,5f);
            // Debug.DrawLine(lastVert, currentVert, Color.blue,5f);
            // Debug.DrawLine(currentVert,currentShape[3], Color.magenta,5f);
            if(intersection.Item1 && intersection.Item2)
            {
                currentShape.Insert(0,localCenter);
                currentShape.Insert(1,intersection.Item4);
                foundCollisionPoint = intersection.Item4;
                break;
            }
            else
            {
                currentShape.RemoveAt(0);
                vertexToClear = vertexToClear + 1;
            }
            if(currentShape.Count<2)
            {
                break;
            }
            lastVert = currentShape[0];
            currentVert =  currentShape[1];
        }


        var tempVertexMap = new Vector3[currentVertexMap.Length-(3*vertexToClear)];
        var tempTriangleMap = new int[currentVertexMap.Length-(3*vertexToClear)];
        var tempUVMap = new Vector2[currentVertexMap.Length-(3*vertexToClear)];
        for(var i = 0;i<tempVertexMap.Length;i++)
        {
            tempVertexMap[i] = currentVertexMap[i + (3*vertexToClear)];
            tempTriangleMap[i] = currentTriangleMap[i + (3*vertexToClear)] - (3*vertexToClear);
            tempUVMap[i] = currentUVMap[i + (3*vertexToClear)];
        }
        
        currentVertexMap = tempVertexMap;
        currentTriangleMap = tempTriangleMap;
        currentUVMap = tempUVMap;

        //currentVertexMap[1] = VectorSpaceConversionUtility.vector2ToVector3(foundCollisionPoint);
        currentVertexMap[2] = VectorSpaceConversionUtility.vector2ToVector3(foundCollisionPoint);

        var mesh = new Mesh();
        mesh.vertices = currentVertexMap;
        mesh.triangles = currentTriangleMap;
        mesh.uv = currentUVMap;

        cooldownIndicator.GetComponent<MeshFilter>().mesh = mesh;
        
        currentShape.RemoveAt(0);
        currentShape.RemoveAt(0);
    }
}
