using UnityEngine;
public static class VectorSpaceConversionUtility
{
    public static Vector3 vector2ToVector3(Vector2 vector, float setZVal = 0f)
    {
        return new Vector3(vector[0], vector[1], setZVal);
    }
    public static Vector2 vector3ToVector2(Vector3 vector)
    {
        return new Vector2(vector[0], vector[1]);
    }
    public static Vector2 divideVector(Vector2 top, Vector2 bottom)
    {
        return new Vector2(top[0] / bottom[0], top[1] / bottom[1]);
    }
    public static Vector3 divideVector(Vector3 top, Vector3 bottom)
    {
        return new Vector3(top[0] / bottom[0], top[1] / bottom[1], top[2] / bottom[2]);
    }
    public static Vector3 transferVectorSpace(Vector3 vec, Transform from = null, Transform to = null) //null is world space
    {
        var newVector = new Vector3(vec[0], vec[1], vec[2]);
        if (from != null)
        {
            newVector = Vector3.Scale(newVector, from.lossyScale);
            newVector = from.rotation * newVector;
            newVector = newVector + from.position;
        }
        if (to != null)
        {

            newVector = newVector - to.position;
            newVector = Quaternion.Inverse(to.rotation) * newVector;
            newVector = divideVector(newVector, to.lossyScale);
        }
        return newVector;
    }
}