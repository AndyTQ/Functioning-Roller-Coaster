using UnityEngine;

namespace WSMGameStudio.Splines
{
    public enum BezierControlPointMode
    {
        Aligned,
        Mirrored,
        Free
    }

    public enum SplineFollowerMode
    {
        StopAtTheEnd,
        Loop,
        PingPong
    }

    public enum MessageType
    {
        Success,
        Error,
        Warning
    }

    public static class SplineDefaultValues
    {
        public const int StepsPerCurve = 10;
        public const float DirectionScale = 0.5f;
    }

    public static class Convert
    {
        public static Quaternion Vector3ToQuaternion(Vector3 v3)
        {
            return Quaternion.Euler(v3);
        }

        public static Vector3 QuaternionToVector3(Quaternion q)
        {
            return q.eulerAngles;
        }

        public static Quaternion Vector4ToQuaternion(Vector4 v4)
        {
            return new Quaternion(v4.x, v4.y, v4.z, v4.w);
        }

        public static Vector4 QuaternionToVector4(Quaternion q)
        {
            return new Vector4(q.x, q.y, q.z, q.w);
        }
    }

    public static class FloatArrayExtensions
    {
        /// <summary>
        /// Table for distance calculation
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="bezier"></param>
        /// <returns></returns>
        //public static float[] CalcLengthTableInfo(float[] arr, Spline bezier)
        //{
        //    arr[0] = 0f;
        //    float totalLength = 0f;
        //    Vector3 prev = bezier.GetPoint(0);
        //    for (int i = 1; i < arr.Length; i++)
        //    {
        //        float t = ((float)i) / (arr.Length - 1);
        //        Vector3 pt = bezier.GetPoint(t);
        //        float diff = (prev - pt).magnitude;
        //        totalLength += diff;
        //        arr[i] = totalLength;
        //        prev = pt;
        //    }

        //    return arr;
        //}

        ///// <summary>
        ///// Get interpolated value beween points
        ///// </summary>
        ///// <param name="fArr"></param>
        ///// <param name="t"></param>
        ///// <returns></returns>
        //public static float Sample(this float[] fArr, float t)
        //{
        //    int count = fArr.Length;
        //    if (count == 0)
        //    {
        //        Debug.LogError("Unable to sample array - it has no elements");
        //        return 0f;
        //    }
        //    if (count == 1)
        //        return fArr[0];

        //    float iFloat = t * (count - 1);
        //    int idLower = Mathf.FloorToInt(iFloat);
        //    int idUpper = Mathf.FloorToInt(iFloat + 1);
        //    if (idUpper >= count)
        //        return fArr[count - 1];
        //    if (idLower < 0)
        //        return fArr[0];

        //    return Mathf.Lerp(fArr[idLower], fArr[idUpper], iFloat - idLower);
        //}
    }
}