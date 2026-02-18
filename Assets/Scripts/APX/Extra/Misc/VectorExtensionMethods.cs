using UnityEngine;

namespace APX.Extra.Misc
{
    public static class VectorExtensionMethods
    {
        public static Vector2 xy(this Vector3 v) { return new Vector2(v.x, v.y); }
        public static Vector2 xz(this Vector3 v) { return new Vector2(v.x, v.z); }
        public static Vector2 yz(this Vector3 v) { return new Vector2(v.y, v.z); }
        public static Vector2 yx(this Vector3 v) { return new Vector2(v.y, v.x); }
        public static Vector2 zx(this Vector3 v) { return new Vector2(v.z, v.x); }
        public static Vector2 zy(this Vector3 v) { return new Vector2(v.z, v.y); }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            v.x = x;
            return v;
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            v.y = y;
            return v;
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            v.z = z;
            return v;
        }

        public static Vector3 WithXY(this Vector3 v, float x, float y)
        {
            v.x = x;
            v.y = y;
            return v;
        }

        public static Vector3 WithXZ(this Vector3 v, float x, float z)
        {
            v.x = x;
            v.z = z;
            return v;
        }

        public static Vector3 WithYZ(this Vector3 v, float y, float z)
        {
            v.y = y;
            v.z = z;
            return v;
        }

        public static Vector2 WithX(this Vector2 v, float x)
        {
            v.x = x;
            return v;
        }

        public static Vector2 WithY(this Vector2 v, float y)
        {
            v.y = y;
            return v;
        }

        public static Vector3 WithZ(this Vector2 v, float z) { return new Vector3(v.x, v.y, z); }

        public static Vector4 WithX(this Vector4 v, float x)
        {
            v.x = x;
            return v;
        }

        public static Vector4 WithY(this Vector4 v, float y)
        {
            v.y = y;
            return v;
        }

        public static Vector4 WithZ(this Vector4 v, float z)
        {
            v.z = z;
            return v;
        }

        public static Vector4 WithW(this Vector4 v, float w)
        {
            v.w = w;
            return v;
        }

        public static Vector3 ScaledToOne(this Vector3 v) { return new Vector3(Mathf.Sign(v.x), Mathf.Sign(v.y), Mathf.Sign(v.z)); }

        public static Vector2 Random(this Vector2 v, float min = -1, float max = 1)
        {
            v.x = UnityEngine.Random.Range(min, max);
            v.y = UnityEngine.Random.Range(min, max);
            return v;
        }

        public static Vector3 Random(this Vector3 v, float min = -1, float max = 1)
        {
            v.x = UnityEngine.Random.Range(min, max);
            v.y = UnityEngine.Random.Range(min, max);
            v.z = UnityEngine.Random.Range(min, max);
            return v;
        }


    #region With including adding
        public static Vector3 WithAddX(this Vector3 v, float addedValue)
        {
            v.x += addedValue;
            return v;
        }

        public static Vector3 WithAddY(this Vector3 v, float addedValue)
        {
            v.y += addedValue;
            return v;
        }

        public static Vector3 WithAddZ(this Vector3 v, float addedValue)
        {
            v.z += addedValue;
            return v;
        }

        public static Vector3 WithAddXY(this Vector3 v, float addX, float addY)
        {
            v.x += addX;
            v.y += addY;
            return v;
        }

        public static Vector3 WithAddXZ(this Vector3 v, float addX, float addZ)
        {
            v.x += addX;
            v.y += addZ;
            return v;
        }

        public static Vector3 WithAddYZ(this Vector3 v, float addY, float addZ)
        {
            v.x += addY;
            v.y += addZ;
            return v;
        }

        public static Vector3 WithAddAll(this Vector3 v, float addX, float addY, float addZ)
        {
            v.x += addX;
            v.y += addY;
            v.z += addZ;
            return v;
        }

        public static Vector3 WithAddAll(this Vector3 v, float addedValue)
        {
            v.x += addedValue;
            v.y += addedValue;
            v.z += addedValue;
            return v;
        }

        public static Vector2 WithAddX(this Vector2 v, float addedValue)
        {
            v.x += addedValue;
            return v;
        }

        public static Vector2 WithAddY(this Vector2 v, float addedValue)
        {
            v.y += addedValue;
            return v;
        }

        public static Vector2 WithAddAll(this Vector2 v, float addX, float addY)
        {
            v.x += addX;
            v.y += addY;
            return v;
        }

        public static Vector2 WithAddAll(this Vector2 v, float addedValue)
        {
            v.x += addedValue;
            v.y += addedValue;
            return v;
        }
    #endregion


    #region With including multiplication
        public static Vector3 WithMultX(this Vector3 v, float multiplyBy)
        {
            v.x *= multiplyBy;
            return v;
        }

        public static Vector3 WithMultY(this Vector3 v, float multiplyBy)
        {
            v.y *= multiplyBy;
            return v;
        }

        public static Vector3 WithMultZ(this Vector3 v, float multiplyBy)
        {
            v.z *= multiplyBy;
            return v;
        }

        public static Vector3 WithMultXY(this Vector3 v, float multX, float multY)
        {
            v.x *= multX;
            v.y *= multY;
            return v;
        }

        public static Vector3 WithMultXZ(this Vector3 v, float multX, float multZ)
        {
            v.x *= multX;
            v.z *= multZ;
            return v;
        }

        public static Vector3 WithMultYZ(this Vector3 v, float multY, float multZ)
        {
            v.y *= multY;
            v.z *= multZ;
            return v;
        }

        public static Vector3 WithMultAll(this Vector3 v, float multX, float multY, float multZ)
        {
            v.x *= multX;
            v.y *= multY;
            v.z *= multZ;
            return v;
        }

        public static Vector2 WithMultX(this Vector2 v, float multiplyBy)
        {
            v.x *= multiplyBy;
            return v;
        }

        public static Vector2 WithMultY(this Vector2 v, float multiplyBy)
        {
            v.y *= multiplyBy;
            return v;
        }

        public static Vector2 WithMultAll(this Vector2 v, float multX, float multY)
        {
            v.x *= multX;
            v.y *= multY;
            return v;
        }
    #endregion


    #region Dimensions
        public static float Get(this ref Vector3 v, Dimension dimension)
        {
            switch (dimension)
            {
                case Dimension.x:
                    return v.x;
                case Dimension.y:
                    return v.y;
                case Dimension.z:
                    return v.z;
                default:
                    return 0f;
            }
        }

        public static void Set(this ref Vector3 v, Dimension dimension, float value)
        {
            switch (dimension)
            {
                case Dimension.x:
                    v.x = value;
                    return;
                case Dimension.y:
                    v.y = value;
                    return;
                case Dimension.z:
                    v.z = value;
                    return;
                default:
                    return;
            }
        }

        public static void Add(this ref Vector3 v, Dimension dimension, float value)
        {
            switch (dimension)
            {
                case Dimension.x:
                    v.x += value;
                    return;
                case Dimension.y:
                    v.y += value;
                    return;
                case Dimension.z:
                    v.z += value;
                    return;
                default:
                    return;
            }
        }

        public static void Multiply(this ref Vector3 v, Dimension dimension, float value)
        {
            switch (dimension)
            {
                case Dimension.x:
                    v.x *= value;
                    return;
                case Dimension.y:
                    v.y *= value;
                    return;
                case Dimension.z:
                    v.z *= value;
                    return;
                default:
                    return;
            }
        }

        public static float Get(this ref Vector2 v, Dimension dimension)
        {
            switch (dimension)
            {
                case Dimension.x:
                    return v.x;
                case Dimension.y:
                    return v.y;
                default:
                    return 0f;
            }
        }

        public static void Set(this ref Vector2 v, Dimension dimension, float value)
        {
            switch (dimension)
            {
                case Dimension.x:
                    v.x = value;
                    return;
                case Dimension.y:
                    v.y = value;
                    return;
                default:
                    return;
            }
        }

        public static void Add(this ref Vector2 v, Dimension dimension, float value)
        {
            switch (dimension)
            {
                case Dimension.x:
                    v.x += value;
                    return;
                case Dimension.y:
                    v.y += value;
                    return;
                default:
                    return;
            }
        }

        public static void Multiply(this ref Vector2 v, Dimension dimension, float value)
        {
            switch (dimension)
            {
                case Dimension.x:
                    v.x *= value;
                    return;
                case Dimension.y:
                    v.y *= value;
                    return;
                default:
                    return;
            }
        }
    #endregion


    #region Clamping
        public static Vector3 ClampX(this Vector3 v, float minX, float maxX)
        {
            var value = v.x;
            if (value < minX) v.x = minX;
            else if (value > maxX) v.x = maxX;

            return v;
        }

        public static Vector3 ClampX(this Vector3 v, Vector2 minMax)
        {
            var value = v.x;
            if (value < minMax.x) v.x = minMax.x;
            else if (value > minMax.y) v.x = minMax.y;

            return v;
        }

        public static Vector3 ClampY(this Vector3 v, float minY, float maxY)
        {
            var value = v.y;
            if (value < minY) v.y = minY;
            else if (value > maxY) v.y = maxY;

            return v;
        }

        public static Vector3 ClampY(this Vector3 v, Vector2 limits)
        {
            var value = v.y;
            if (value < limits.x) v.y = limits.x;
            else if (value > limits.y) v.y = limits.y;

            return v;
        }

        public static Vector3 ClampZ(this Vector3 v, float minZ, float maxZ)
        {
            var value = v.z;
            if (value < minZ) v.z = minZ;
            else if (value > maxZ) v.z = maxZ;

            return v;
        }

        public static Vector3 ClampZ(this Vector3 v, Vector2 minMax)
        {
            var value = v.z;
            if (value < minMax.x) v.z = minMax.x;
            else if (value > minMax.y) v.z = minMax.y;

            return v;
        }

        public static Vector3 ClampXY(this Vector3 v, float minX, float maxX, float minY, float maxY)
        {
            var value = v.x;
            if (value < minX) v.x = minX;
            else if (value > maxX) v.x = maxX;

            value = v.y;
            if (value < minY) v.y = minY;
            else if (value > maxY) v.y = maxY;

            return v;
        }

        public static Vector3 ClampXY(this Vector3 v, Vector2 minMaxX, Vector2 minMaxY)
        {
            var value = v.x;
            if (value < minMaxX.x) v.x = minMaxX.x;
            else if (value > minMaxX.y) v.x = minMaxX.y;

            value = v.y;
            if (value < minMaxY.x) v.y = minMaxY.x;
            else if (value > minMaxY.y) v.y = minMaxY.y;

            return v;
        }

        public static Vector3 ClampXZ(this Vector3 v, float minX, float maxX, float minZ, float maxZ)
        {
            var value = v.x;
            if (value < minX) v.x = minX;
            else if (value > maxX) v.x = maxX;

            value = v.z;
            if (value < minZ) v.z = minZ;
            else if (value > maxZ) v.z = maxZ;

            return v;
        }

        public static Vector3 ClampXZ(this Vector3 v, Vector2 minMaxX, Vector2 minMaxZ)
        {
            var value = v.x;
            if (value < minMaxX.x) v.x = minMaxX.x;
            else if (value > minMaxX.y) v.x = minMaxX.y;

            value = v.z;
            if (value < minMaxZ.x) v.z = minMaxZ.x;
            else if (value > minMaxZ.y) v.z = minMaxZ.y;

            return v;
        }

        public static Vector3 ClampYZ(this Vector3 v, float minY, float maxY, float minZ, float maxZ)
        {
            var value = v.y;
            if (value < minY) v.y = minY;
            else if (value > maxY) v.y = maxY;

            value = v.z;
            if (value < minZ) v.z = minZ;
            else if (value > maxZ) v.z = maxZ;

            return v;
        }

        public static Vector3 ClampYZ(this Vector3 v, Vector2 minMaxY, Vector2 minMaxZ)
        {
            var value = v.y;
            if (value < minMaxY.x) v.y = minMaxY.x;
            else if (value > minMaxY.y) v.y = minMaxY.y;

            value = v.z;
            if (value < minMaxZ.x) v.z = minMaxZ.x;
            else if (value > minMaxZ.y) v.z = minMaxZ.y;

            return v;
        }

        public static Vector3 ClampAll(this Vector3 v, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
        {
            var value = v.x;
            if (value < minX) v.x = minX;
            else if (value > maxX) v.x = maxX;

            value = v.y;
            if (value < minY) v.y = minY;
            else if (value > maxY) v.y = maxY;

            value = v.z;
            if (value < minZ) v.z = minZ;
            else if (value > maxZ) v.z = maxZ;

            return v;
        }

        public static Vector3 ClampAll(this Vector3 v, Vector2 minMaxX, Vector2 minMaxY, Vector2 minMaxZ)
        {
            var value = v.x;
            if (value < minMaxX.x) v.x = minMaxX.x;
            else if (value > minMaxX.y) v.x = minMaxX.y;

            value = v.y;
            if (value < minMaxY.x) v.y = minMaxY.x;
            else if (value > minMaxY.y) v.y = minMaxY.y;

            value = v.z;
            if (value < minMaxZ.x) v.z = minMaxZ.x;
            else if (value > minMaxZ.y) v.z = minMaxZ.y;

            return v;
        }

        public static Vector3 ClampAll(this Vector3 v, Vector2 minMax)
        {
            var value = v.x;
            if (value < minMax.x) v.x = minMax.x;
            else if (value > minMax.y) v.x = minMax.y;

            value = v.y;
            if (value < minMax.x) v.y = minMax.x;
            else if (value > minMax.y) v.y = minMax.y;

            value = v.z;
            if (value < minMax.x) v.z = minMax.x;
            else if (value > minMax.y) v.z = minMax.y;

            return v;
        }
    #endregion


    #region Element-wise operations
        public static Vector3 MultiplyElementWise(this Vector3 v, Vector3 u)
        {
            v.x *= u.x;
            v.y *= u.y;
            v.z *= u.z;
            return v;
        }

        public static Vector3 MultiplyElementWise(this Vector2 v, Vector2 u) { return new Vector3(v.x * u.x, v.y * u.y); }

        public static Vector3 DivideElementWise(this Vector3 v, Vector3 u)
        {
            v.x /= u.x;
            v.y /= u.y;
            v.z /= u.z;
            return v;
        }

        public static Vector3 DivideElementWise(this Vector2 v, Vector2 u) { return new Vector3(v.x / u.x, v.y / u.y); }
    #endregion


        public static Vector3 Towards(this Vector3 v, Vector3 v2, float normalizedDistance)
        {
            v.x += (v2.x - v.x) * normalizedDistance;
            v.y += (v2.y - v.y) * normalizedDistance;
            v.z += (v2.z - v.z) * normalizedDistance;
            return v;
        }

        public static Vector3 InBetween(this Vector3 v, Vector3 v2) { return v.Towards(v2, 0.5f); }

        // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
        {
            if (!isNormalized) axisDirection.Normalize();
            var d = Vector3.Dot(point, axisDirection);
            return axisDirection * d;
        }

        // lineDirection - unit vector in direction of line
        // pointOnLine - a point on the line (allowing us to define an actual line in space)
        // point - the point to find nearest on line for
        public static Vector3 NearestPointOnLine(this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
        {
            if (!isNormalized) lineDirection.Normalize();
            var d = Vector3.Dot(point - pointOnLine, lineDirection);
            return pointOnLine + (lineDirection * d);
        }

        public static Vector2 ProjectOn(this Vector2 vector, Vector2 onNormal)
        {
            var tmp = (vector.x * onNormal.x + vector.y * onNormal.y) / (onNormal.x * onNormal.x + onNormal.y * onNormal.y);
            return new Vector2(tmp * onNormal.x, tmp * onNormal.y);
        }

        public static float MinAbsComponent(this Vector3 vec) { return Mathf.Min(Mathf.Abs(vec.x), Mathf.Abs(vec.y), Mathf.Abs(vec.z)); }

        /// <summary>
        /// Return whether a point is inside a triangle in 2D
        /// </summary>
        /// <param name="p">Point to test</param>
        /// <param name="t0">Vertex of the triangle</param>
        /// <param name="t1">Vertex of the triangle</param>
        /// <param name="t3">Vertex of the triangle</param>
        /// <returns></returns>
        public static bool PointInTriangle2D(this Vector2 p, Vector2 t0, Vector2 t1, Vector2 t3)
        {
            var s = t0.y * t3.x - t0.x * t3.y + (t3.y - t0.y) * p.x + (t0.x - t3.x) * p.y;
            var t = t0.x * t1.y - t0.y * t1.x + (t0.y - t1.y) * p.x + (t1.x - t0.x) * p.y;

            if (s < 0 != t < 0)
                return false;

            var A = -t1.y * t3.x + t0.y * (t3.x - t1.x) + t0.x * (t1.y - t3.y) + t1.x * t3.y;

            return A < 0 ? s <= 0 && s + t >= A : s >= 0 && s + t <= A;
        }
    }
}
