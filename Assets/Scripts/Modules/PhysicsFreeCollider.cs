using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;



public class PhysicsFreeCollider : MonoBehaviour
{
    public Transform defaultRelative;
    public List<List<Vector2>> shapeList; // use setHitBox to set it is public to get

    public bool hasHitBox;

    public bool isNegativeHitBox = false;

    private Vector2 topRight;
    private Vector2 bottomLeft;

    private List<List<Vector2>> maskShape;
    private Vector2 maskTopRight;
    private Vector2 maskBottomLeft;

    void Awake()
    {
        hasHitBox = false;
    }

    public void setHitBox(List<List<Vector2>> shapes, Transform relativeTo, bool canKeepList = true, bool makeMaskable = true)
    {
        
        if(shapes == null){return;}
        if(shapes.Count<1){return;}
        if(shapes[0].Count<3){return;}
        hasHitBox = false;
        defaultRelative = relativeTo;
        //Debug.Log("num shapes: "+ shapes.Count);
        
        if(canKeepList)
        {
            shapeList = shapes;
        }
        else
        {
           shapeList = copyList(shapes); 
        }
        for(var i = shapeList.Count-1;i >= 0;i--)
        {
            if(shapeList[i].Count < 1)
            {
                shapeList.RemoveAt(i);
            }
        }
        if(shapeList.Count<1){return;}

        topRight = new Vector2(shapeList[0][0][0],shapeList[0][0][1]);
        bottomLeft = new Vector2(shapeList[0][0][0],shapeList[0][0][1]);
        foreach (var shape in shapeList)
        {
            //var lastPoint = shape[shape.Count-1];
            foreach(var vert in shape)
            {
                //Debug.Log(vert);
                if(topRight[0] < vert[0])
                {
                    topRight[0] = vert[0];
                }
                if(topRight[1] < vert[1])
                {
                    topRight[1] = vert[1];
                }
                if(bottomLeft[0] > vert[0])
                {
                    bottomLeft[0] = vert[0];
                }
                if(bottomLeft[1] > vert[1])
                {
                    bottomLeft[1] = vert[1];
                }
                 //Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(lastPoint),VectorSpaceConversionUtility.vector2ToVector3(vert),Color.red,1000,false);
                 //lastPoint = vert;
            }
        }
        hasHitBox = true;
    }

    public (Vector2,Vector2,List<List<Vector2>>) getShapeData()
    {
        return (topRight,bottomLeft,shapeList);
    }

    public void loadPreComputedShapes((Vector2,Vector2,List<List<Vector2>>) shapeData, Transform relativeTo, bool makeMaskable = true)
    {
       
        topRight = shapeData.Item1;
        bottomLeft = shapeData.Item2;
        shapeList = shapeData.Item3;
        defaultRelative = relativeTo;
        hasHitBox = true;
    }

    public void setHitBox(Transform matchAttachedSpriteRenderer)
    {
        var shapes = HitBoxUtility.calcHitBoxAround(matchAttachedSpriteRenderer);
        setHitBox(shapes,matchAttachedSpriteRenderer);
    }

    public bool isHit(Vector3 pos, Transform relativeTo = null)
    {
        if(!hasHitBox)
		{
            return isNegativeHitBox;
        }
        if(relativeTo == null)
        {
            relativeTo = defaultRelative;
        }
        if(maskShape != null)
        {
            if(!hitShape(maskShape,pos,null,maskTopRight,maskBottomLeft))
            {
                return isNegativeHitBox;
            }
        }
       return hitShape(shapeList,pos,relativeTo,topRight,bottomLeft) != isNegativeHitBox;
    }

    private bool hitShape(List<List<Vector2>> shapes, Vector3 pos, Transform relativeTo, Vector2 tRight, Vector2 bLeft)
    {
        var localPos = VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(pos,null,relativeTo)); 
        
        if(localPos[0] > tRight[0] || localPos[1] > tRight[1] || localPos[0] < bLeft[0] || localPos[1] < bLeft[1])
        {
            return false;
        }
        var intersectionCount = 0;
        foreach (var shape in shapes)
        {
            var lastVert = shape[shape.Count-1];
            foreach(var vert in shape)
            {
                if(localPos[0] == vert[0] && localPos[1]<= vert[1])
                {
                    intersectionCount = intersectionCount + 1;
                    //Debug.Log(relativeTo+ "::::::::Point Hit: " + intersectionCount);
                }
                else
                {
                    if(lastVert != vert)
                    {
                       if(isBetween(localPos[0],lastVert[0],vert[0]))
                       {
                            if(yIntersectionIsBetween(localPos,lastVert,vert))
                            {
                                intersectionCount = intersectionCount + 1;
                               //Debug.Log(relativeTo+ "::::::::Edge Hit: LocalPos: " + localPos + "::: Last Vert: " + lastVert + ":::: Current Vert" + vert);
                            }
                       }
                    }
                }
                //Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(lastVert),VectorSpaceConversionUtility.vector2ToVector3(vert),Color.red,1,false);
//                 var test = HitBoxUtility.getIntersection((lastVert,vert),(Vector2.zero, localPos));
//                 Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(Vector2.zero),VectorSpaceConversionUtility.vector2ToVector3(localPos),Color.blue,1,false);
//                 if(test.Item1 && test.Item2)
//                 {
// Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(test.Item4),VectorSpaceConversionUtility.vector2ToVector3(test.Item4 + new Vector2(1,1)),Color.magenta,1,false);
//                 }
                
                lastVert = vert;
            }
        }
        //Debug.Log(relativeTo + "Total Hits: " + intersectionCount);
        return intersectionCount % 2 == 1;
    }

    public List<(Vector2,((int,int),(Vector2, Vector2)))> getLineIntersections((Vector2,Vector2) l, Transform relativeTo = null)
    {
        if(!hasHitBox)
		{
            return null;
        }
        if(relativeTo == null)
        {
            relativeTo = defaultRelative;
        }
        var rl = new List<(Vector2,((int,int),(Vector2, Vector2)))>();
        var localLine = (VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(l.Item1,null,relativeTo)), VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(l.Item2,null,relativeTo)));
        var shapeInd = 0;
        
        foreach (var shape in shapeList)
        {
            var vertInd = 0;
            var lastVert = shape[shape.Count-1];
            foreach(var vert in shape)
            {
                Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(lastVert),VectorSpaceConversionUtility.vector2ToVector3(vert),Color.red,1,false);
                var inter = HitBoxUtility.getIntersection((lastVert,vert), localLine);
                Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(localLine.Item1),VectorSpaceConversionUtility.vector2ToVector3(localLine.Item2),Color.blue,1,false);
                if(inter.Item1 && inter.Item2)
                {
                    Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(inter.Item4),VectorSpaceConversionUtility.vector2ToVector3(inter.Item4 + new Vector2(1,1)),Color.magenta,1,false);
                    rl.Add((VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(inter.Item4,relativeTo,null)),((shapeInd,vertInd),(VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(lastVert,relativeTo,null)),VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(vert,relativeTo,null))))));
                }
                
                lastVert = vert;
                vertInd = vertInd + 1;
            }
            shapeInd = shapeInd + 1;
        }
        return rl;
    }

    public bool areaHit(Transform area, Transform relativeTo = null)
    {
        if (!hasHitBox)
        {
            return false;
        }
        if (area == null)
        {
            return false;
        }
        if (area.GetComponent<PhysicsFreeCollider>() == null)
        {
            return false;
        }
        if (relativeTo == null)
        {
            relativeTo = defaultRelative;
        }
        var areaCollider = area.GetComponent<PhysicsFreeCollider>();
        var areaCenter = areaCollider.getCenter();
        var areaExtents = areaCollider.getExtents();
        var selfCenter = getCenter();
        var selfExtents = getExtents();
        var collisionBox = HitBoxUtility.getOverlapBox((areaCenter + areaExtents, areaCenter - areaExtents), (selfCenter + selfExtents, selfCenter - selfExtents));
        //Debug.DrawLine(collisionBox.Item1, collisionBox.Item2 , Color.red, 1f, false);
        var boxDiff = collisionBox.Item1- collisionBox.Item2 ;
        if (boxDiff[0]<0f || boxDiff[1]<0f)
        {
            //Debug.Log("no box overlap");
            return false;
        }
        if (areaCollider.isHit(VectorSpaceConversionUtility.transferVectorSpace(shapeList[0][0], relativeTo, null)))
        {
            //Debug.Log("point in area");
            return true;
        }
        if (isHit(VectorSpaceConversionUtility.transferVectorSpace(areaCollider.shapeList[0][0], area, null)))
        {
            //Debug.Log("area point in shape");
            return true;
        }
        var localCollisionBox = (VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(VectorSpaceConversionUtility.vector2ToVector3(collisionBox.Item1), null, relativeTo)) + new Vector2(.01f,.01f), VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(VectorSpaceConversionUtility.vector2ToVector3(collisionBox.Item2), null, relativeTo))- new Vector2(.01f,.01f));
        var areaLocalCollisionBox = (VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(VectorSpaceConversionUtility.vector2ToVector3(collisionBox.Item1), null, area)) + new Vector2(.01f,.01f), VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(VectorSpaceConversionUtility.vector2ToVector3(collisionBox.Item2), null, area))- new Vector2(.01f,.01f));
        var areaLinesInBox = new List<(Vector2, Vector2)>();
        var linesInBox = new List<(Vector2, Vector2)>();
        foreach (var shape in areaCollider.shapeList)
        {
            var lastVert = shape[shape.Count - 1];
            foreach (var vert in shape)
            {
                if (HitBoxUtility.lineInBox((lastVert, vert), areaLocalCollisionBox))
                {
                    areaLinesInBox.Add((lastVert, vert));
                }
                lastVert = vert;
            }
        }
        foreach (var shape in shapeList)
        {
            var lastVert = shape[shape.Count - 1];
            foreach (var vert in shape)
            {
                if (HitBoxUtility.lineInBox((lastVert, vert), localCollisionBox))
                {
                    linesInBox.Add((lastVert, vert));
                }
                lastVert = vert;
            }
        }
        //Debug.Log("Lines in Box: " + linesInBox.Count + " : Area Lines in Box: " + areaLinesInBox.Count);
        foreach (var localLine in linesInBox)
        {
            var transferredLine = (VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(VectorSpaceConversionUtility.vector2ToVector3(localLine.Item1), relativeTo, area)), VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(VectorSpaceConversionUtility.vector2ToVector3(localLine.Item2), relativeTo, area)));
            //Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(transferredLine.Item1), VectorSpaceConversionUtility.vector2ToVector3(transferredLine.Item2), Color.blue, 1f, false);

            foreach (var areaLocalLine in areaLinesInBox)
            {

                //Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(areaLocalLine.Item1), VectorSpaceConversionUtility.vector2ToVector3(areaLocalLine.Item2), Color.green, 1f, false);
                var inter = HitBoxUtility.getIntersection(transferredLine, areaLocalLine);
                if (inter.Item1 && inter.Item2)
                {
                    //Debug.DrawLine(inter.Item4, inter.Item4 + new Vector2(1, 1), Color.magenta, 1f, false);
                    return true;
                }
            }
        }
        return false;
    }

    

    public void containInMaskShape(List<List<Vector2>> shapes)
    {
        maskShape = shapes;
        for (var i = 0; i < maskShape.Count; i++)
        {
            var shape = maskShape[i];
            for (var j = 0; j < shape.Count; j++)
            {
                var vert = shape[j];
                if (maskTopRight[0] < vert[0])
                {
                    maskTopRight[0] = vert[0];
                }
                if (maskTopRight[1] < vert[1])
                {
                    maskTopRight[1] = vert[1];
                }
                if (maskBottomLeft[0] > vert[0])
                {
                    maskBottomLeft[0] = vert[0];
                }
                if (maskBottomLeft[1] > vert[1])
                {
                    maskBottomLeft[1] = vert[1];
                }
            }
        }
    }
    public void containInSquareMask(Vector2 maskTopRightWorldSpace, Vector2 maskBottomLeftWorldSpace)
    {
        var shapes = new List<List<Vector2>>();
        shapes.Add(new List<Vector2>());
        shapes[0].Add(maskTopRightWorldSpace);
        shapes[0].Add(new Vector2(maskBottomLeftWorldSpace[0],maskTopRightWorldSpace[1]));
        shapes[0].Add(maskBottomLeftWorldSpace);
        shapes[0].Add(new Vector2(maskTopRightWorldSpace[0],maskBottomLeftWorldSpace[1]));
        containInMaskShape(shapes);
    }

    private bool yIntersectionIsBetween(Vector2 p, Vector2 b1, Vector2 b2)
    { 
        if(p[1]>b1[1]&&p[1]>b2[1])
        {
            return false;
        }
        if((b1[0]-b2[0]) == 0f)
        {
            return false;
        }
        var slope = (b1[1]-b2[1])/(b1[0]-b2[0]);
        var yIntercept = b1[1] - b1[0]*slope;
        var yIntersection = (p[0]*slope) + yIntercept;
        if(p[1]>yIntersection)
        {
            return false;
        }
        //Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(p),new Vector3(p[0],yIntersection,0f),Color.yellow,1f,false);
        //Debug.DrawLine(new Vector3(p[0],yIntersection,0f),new Vector3(p[0]+1f,yIntersection,0f),Color.green,1f,false);
        return isBetween(yIntersection, b1[1], b2[1]) || b1[1]==yIntersection || yIntersection == b2[1];
    }
    private bool isBetween(float p, float b1, float b2)
    {
        if(b1>b2)
        {
            return b1>p && b2<p;
        }
        else
        {
            return b2>p && b1<p;
        }
    }
    public Vector2 getExtents(Transform relativeTo = null)
    {
        if(relativeTo == null)
        {
            relativeTo = defaultRelative;
        }
       return relativeTo.rotation * Vector3.Scale(VectorSpaceConversionUtility.vector2ToVector3(getLocalExtents()), relativeTo.lossyScale);
    }
    public Vector2 getCenter(Transform relativeTo = null)
    {
       if(relativeTo == null)
        {
            relativeTo = defaultRelative;
        }
        return VectorSpaceConversionUtility.vector3ToVector2(VectorSpaceConversionUtility.transferVectorSpace(VectorSpaceConversionUtility.vector2ToVector3(getLocalCenter()),relativeTo,null));
    }
    public Vector2 getLocalExtents()
    {
        return new Vector2(topRight[0],topRight[1]) - getLocalCenter();
    }
    public Vector2 getLocalCenter()
    {
        return new Vector2((topRight[0] + bottomLeft[0])/2f,(topRight[1] + bottomLeft[1])/2f);
    }

    

    private List<List<Vector2>> copyList(List<List<Vector2>> shapes)
    {
        var list = new List<List<Vector2>>();
        foreach (var shape in shapes)
        {
            var newShape = new List<Vector2>();
            foreach(var vert in shape)
            {
                newShape.Add(new Vector2(vert[0],vert[1]));
            }
            list.Add(newShape);
        }
        return list;
    }
}