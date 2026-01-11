using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public static class HitBoxUtility
{
    public static List<List<Vector2>> calcHitBoxAround(Transform ObjectToMatch)
    {
        if (ObjectToMatch == null) { return null; }
        if (ObjectToMatch.transform.GetComponent<SpriteRenderer>() == null) { return null; }
        if (ObjectToMatch.transform.GetComponent<SpriteRenderer>().sprite == null) { return null; }

        var sprite = ObjectToMatch.transform.GetComponent<SpriteRenderer>().sprite;
        var shapeCount = sprite.GetPhysicsShapeCount();
        var hitBox = new List<List<Vector2>>();
        for (var i = 0; i < shapeCount; i++)
        {
            var list = new List<Vector2>();
            sprite.GetPhysicsShape(i, list);
            hitBox.Add(list);
        }
        return hitBox;
    } 

    public static (bool, bool, bool, Vector2) getIntersection((Vector2, Vector2) l1, (Vector2, Vector2) l2)// returns (canIntersect, intersectionInRange, isParallel, intersectionPoint)
    {
        if (l1.Item1[0] == l1.Item2[0] && l2.Item1[0] == l2.Item2[0]) { return (l1.Item1[0] == l2.Item1[0], getLineOverlap(l1, l2).Item1, true, l1.Item1); }
        var lData1 = Vector2.zero;
        var lData2 = Vector2.zero;
        if (l1.Item1[0] != l1.Item2[0])
        {
            lData1[0] = (l1.Item1[1] - l1.Item2[1]) / (l1.Item1[0] - l1.Item2[0]);
            lData1[1] = l1.Item1[1] - (l1.Item1[0] * lData1[0]);
        }
        if (l2.Item1[0] != l2.Item2[0])
        {
            lData2[0] = (l2.Item1[1] - l2.Item2[1]) / (l2.Item1[0] - l2.Item2[0]);
            lData2[1] = l2.Item1[1] - (l2.Item1[0] * lData2[0]);
        }
        if (lData1[0] == 0f && lData2[0] == 0f)
        { //either 0 could actually be infinite
            //axis alined perpendicular lines
            if (l1.Item1[0] == l1.Item2[0])
            {
                return ((l2.Item1[0] <= l1.Item1[0] && l1.Item1[0] <= l2.Item2[0]) || (l2.Item2[0] <= l1.Item1[0] && l1.Item1[0] <= l2.Item1[0]), (l1.Item1[1] <= l2.Item1[1] && l2.Item1[1] <= l1.Item2[1]) || (l1.Item2[1] <= l2.Item1[1] && l2.Item1[1] <= l1.Item1[1]), false, new Vector2(l1.Item1[0], l2.Item1[1]));
            }
            if (l2.Item1[0] == l2.Item2[0])
            {
                return ((l1.Item1[0] <= l2.Item1[0] && l2.Item1[0] <= l1.Item2[0]) || (l1.Item2[0] <= l2.Item1[0] && l2.Item1[0] <= l1.Item1[0]), (l2.Item1[1] <= l1.Item1[1] && l1.Item1[1] <= l2.Item2[1]) || (l2.Item2[1] <= l1.Item1[1] && l1.Item1[1] <= l2.Item1[1]), false, new Vector2(l2.Item1[0], l1.Item1[1]));
            }
        }
        if (lData1[0] == lData2[0])
        {
            var (isOverlapped, overlapPoint) = getLineOverlap(l1, l2);
            //Debug.Log("paralell");
            return (l1.Item1[0] == l2.Item1[0], isOverlapped, true, overlapPoint);
        }
        if (l1.Item1[0] == l1.Item2[0])
        {
            var p = new Vector2(l1.Item1[0], (lData2[0] * l1.Item1[0]) + lData2[1]);
            //Debug.Log("1 vert");
            return (true, isInRange(p, l1) && isInRange(p, l2), false, p);
        }
        if (l2.Item1[0] == l2.Item2[0])
        {
            var p = new Vector2(l2.Item1[0], (lData1[0] * l2.Item1[0]) + lData1[1]);
            //Debug.Log("2 vert");
            return (true, isInRange(p, l1) && isInRange(p, l2), false, p);
        }
        //Debug.Log("normal");
        var x = (lData1[1] - lData2[1]) / (lData2[0] - lData1[0]);
        var ip = new Vector2(x, (lData1[0] * x) + lData1[1]);
        return (true, (isInRange(ip, l1) && isInRange(ip, l2)), false, ip);
    }

    private static bool isInRange(Vector2 p, (Vector2, Vector2) l)
    {
        return minDistToLine(p, l) < .01f;
    }

    private static (bool, Vector2) getLineOverlap((Vector2, Vector2) l1, (Vector2, Vector2) l2) // only use for parallel lines
    {
        if (isInRange(l1.Item1, l2))
        {
            return (true, l1.Item1);
        }
        if (isInRange(l1.Item2, l2))
        {
            return (true, l1.Item2);
        }
        if (isInRange(l2.Item1, l1))
        {
            return (true, l2.Item1);
        }
        if (isInRange(l2.Item2, l1))
        {
            return (true, l2.Item2);
        }
        return (false, Vector2.zero);
    }

    public static float minDistToLine(Vector2 p, (Vector2, Vector2) l)
    {
        var h = Vector2.Distance(p, l.Item1);
        var theta = Mathf.Abs(getAngleBetweenVectors(p - l.Item1, l.Item2 - l.Item1));
        if (theta >= Mathf.PI / 2f)
        {
            return h;
        }
        if (Mathf.Cos(theta) * h > Vector2.Distance(l.Item1, l.Item2))
        {
            return Vector2.Distance(p, l.Item2);
        }
        return Mathf.Sin(theta) * h;
    }

    public static float getAngleBetweenVectors(Vector2 v1, Vector2 v2)
    {
        var vi = Vector2.Dot(v1, v2) / (v1.magnitude * v2.magnitude);
        if (vi >= 1)
        {
            return 0f;
        }
        return Mathf.Acos(vi);
    }

    public static bool inBox(Vector2 point, (Vector2, Vector2) box) // box must be axis aligned
    {
        return point[0] <= box.Item1[0] && point[0] >= box.Item2[0] && point[1] <= box.Item1[1] && point[1] >= box.Item2[1];
    }

    public static bool lineInBox((Vector2, Vector2) line, (Vector2, Vector2) box) // box must be axis aligned
    {
        if (inBox(line.Item1, box) || inBox(line.Item2, box))
        {
            return true;
        }
        //if end points are both on any one side then line cant be in box
        if (line.Item1[0] < box.Item2[0] && line.Item2[0] < box.Item2[0])
        {
            return false;
        }
        if (line.Item1[0] > box.Item1[0] && line.Item2[0] > box.Item1[0])
        {
            return false;
        }
        if (line.Item1[1] < box.Item2[1] && line.Item2[1] < box.Item2[1])
        {
            return false;
        }
        if (line.Item1[1] > box.Item1[1] && line.Item2[1] > box.Item1[1])
        {
            return false;
        }

        //if line angle is less then angle to any of box corners then line will be in box at some point (requires above check to be done first)
        var lineSlope = Mathf.Abs(getAngleBetweenVectors(new Vector2(1, 0)- line.Item1, line.Item2 - line.Item1));
        if (Mathf.Abs(getAngleBetweenVectors(new Vector2(1, 0)- line.Item1, box.Item1 - line.Item1)) >= lineSlope)
        {
            return true;
        }
        if (Mathf.Abs(getAngleBetweenVectors(new Vector2(1, 0)- line.Item1, box.Item2 - line.Item1)) >= lineSlope)
        {
            return true;
        }
        if (Mathf.Abs(getAngleBetweenVectors(new Vector2(1, 0)- line.Item1, new Vector2(box.Item2[0], box.Item1[1]) - line.Item1)) >= lineSlope)
        {
            return true;
        }
        if (Mathf.Abs(getAngleBetweenVectors(new Vector2(1, 0)- line.Item1, new Vector2(box.Item1[0], box.Item2[1]) - line.Item1)) >= lineSlope)
        {
            return true;
        }
        return false;
    }


    //get a box created from the overlap of two boxes (only works if all boxes are axis aligned)
    public static (Vector2, Vector2) getOverlapBox((Vector2, Vector2) box1, (Vector2, Vector2) box2)
    {
        var minTL = box1.Item1;
        var maxBR = box1.Item2;
        if (box1.Item2[0] < box2.Item2[0])
        {
            maxBR[0] = box2.Item2[0];
        }
        if (box1.Item2[1] < box2.Item2[1])
        {
            maxBR[1] = box2.Item2[1];
        }
        if (box1.Item1[0] > box2.Item1[0])
        {
            minTL[0] = box2.Item1[0];
        }
        if (box1.Item1[1] > box2.Item1[1])
        {
            minTL[1] = box2.Item1[1];
        }
        return (minTL, maxBR);
    }
}
