using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public static class MeshBuilderUtility
{
#if UNITY_EDITOR
    // Example code use this feature with:
    // var mesh = MeshBuilderUtility.lineToMesh(OutLiner.getLinePointsAroundShapeFromProvider((Sprite)Resources.Load("Images/CARD-2back", typeof(Sprite)),0), 3f, .3f, Mathf.PI/4f);
    // MeshBuilderUtility.saveMeshToFile("ResolveGlowCardOutline",mesh);
    public static void saveMeshToFile(string fileNameToSaveAs, Mesh meshToSave)
    {
        var path = "Assets/Assets/Resources/SavedMeshs/" + fileNameToSaveAs + ".asset";
        UnityEditor.AssetDatabase.CreateAsset(meshToSave, path);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.ImportAsset(path);
    }
#endif

    public static Mesh loadSavedMesh(string name)
    {
        return (Mesh)Resources.Load("SavedMeshs/" + name, typeof(Mesh));
    }

    public static Mesh spriteToMesh(Sprite sprite)
    {
        var mesh = new Mesh();
        var vert = new Vector3[sprite.vertices.Length];
        var tri = new int[sprite.triangles.Length];
        for (int i = 0; i < sprite.vertices.Length; i++)
        {
            vert[i] = new Vector3(sprite.vertices[i][0],sprite.vertices[i][1],0f);
        }
        for (int i = 0; i < sprite.triangles.Length; i++)
        {
            tri[i] = (int)sprite.triangles[i];
        }
        mesh.vertices = vert;
        mesh.uv = sprite.uv;
        mesh.triangles = tri;
        return mesh;
    }

    public static Mesh meshFromTwoShapes(List<Vector2> s1, List<Vector2> s2, bool debugLines = false)
    {
        var mesh = new Mesh();

        var s2EmptyOffSet = -1;
        if (s2.Count > 1)
        {
            s2EmptyOffSet = 0;
        }

        var maxVerts = s1.Count;
        var meshVerts = new Vector3[(s1.Count + s2.Count + s2EmptyOffSet) * 3];
        var meshUV = new Vector2[(s1.Count + s2.Count + s2EmptyOffSet) * 3];
        var meshTriangles = new int[(s1.Count + s2.Count + s2EmptyOffSet) * 3];
        var lastVertS1 = s1[s1.Count - 1];
        var lastVertS2 = findClosestPointToLine((s1[s1.Count - 2], s1[s1.Count - 1]), s2);
        var loadedVerts = 0;
        var outerExcessDist = 0f;
        var innerExcessDist = 0f;

        Func<Vector2, Vector2, Vector2, bool, bool, float> addTriangle = (v1, v2, v3, isInner, showDebug) =>
        {
            var dist = Vector2.Distance(v2, v3);
            if (showDebug)
            {
                if (isInner)
                {
                    Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(v3), VectorSpaceConversionUtility.vector2ToVector3(v1), Color.yellow, 1000, false);
                    Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(v1), VectorSpaceConversionUtility.vector2ToVector3(v2), Color.blue, 1000, false);
                    Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(v2), VectorSpaceConversionUtility.vector2ToVector3(v3), Color.white, 1000, false);
                }
                else
                {
                    Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(v3), VectorSpaceConversionUtility.vector2ToVector3(v1), Color.magenta, 1000, false);
                    Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(v1), VectorSpaceConversionUtility.vector2ToVector3(v2), Color.red, 1000, false);
                    Debug.DrawLine(VectorSpaceConversionUtility.vector2ToVector3(v2), VectorSpaceConversionUtility.vector2ToVector3(v3), Color.green, 1000, false);
                }
            }

            if (isInner)
            {
                dist = Vector2.Distance(v1, v2);
                meshUV[loadedVerts] = new Vector2(innerExcessDist + dist, .01f);
                meshUV[loadedVerts + 1] = new Vector2(outerExcessDist, .99f);
                meshUV[loadedVerts + 2] = new Vector2(innerExcessDist, .01f);
            }
            else
            {
                meshUV[loadedVerts] = new Vector2(innerExcessDist + dist, .01f);
                meshUV[loadedVerts + 1] = new Vector2(outerExcessDist + dist, .99f);
                meshUV[loadedVerts + 2] = new Vector2(outerExcessDist, .99f);
            }

            meshVerts[loadedVerts] = VectorSpaceConversionUtility.vector2ToVector3(v1);
            meshTriangles[loadedVerts] = loadedVerts;
            loadedVerts = loadedVerts + 1;
            meshVerts[loadedVerts] = VectorSpaceConversionUtility.vector2ToVector3(v2);
            meshTriangles[loadedVerts] = loadedVerts;
            loadedVerts = loadedVerts + 1;
            meshVerts[loadedVerts] = VectorSpaceConversionUtility.vector2ToVector3(v3);
            meshTriangles[loadedVerts] = loadedVerts;
            loadedVerts = loadedVerts + 1;

            return dist;
        };

        for (var i = 0; i < maxVerts; i++)
        {
            var s1Vert = s1[i];
            var s2Vert = findClosestPointToLine((lastVertS1, s1Vert), s2);
            var outDist = addTriangle(lastVertS2, s1Vert, lastVertS1, false, debugLines);
            outerExcessDist = outDist + outerExcessDist;
            if (s2Vert != lastVertS2)
            {
                var inDist = addTriangle(s2Vert, s1Vert, lastVertS2, true, debugLines);
                innerExcessDist = inDist + innerExcessDist;
            }
            lastVertS1 = s1Vert;
            lastVertS2 = s2Vert;
        }

        mesh.vertices = meshVerts;
        mesh.triangles = meshTriangles;
        mesh.uv = meshUV;

        return mesh;
    }

    private static Vector2 findClosestPointToLine((Vector2, Vector2) l, List<Vector2> shape)
    {
        var min = 1000f;
        var minPoint = Vector2.zero;
        foreach (var s in shape)
        {
            var dist = HitBoxUtility.minDistToLine(s, l);
            if (dist < min)
            {
                minPoint = s;
                min = dist;
            }
        }
        return minPoint;
    }
}
