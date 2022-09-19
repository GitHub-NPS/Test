using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPS
{
    public static class MathParabola
    {
        /// <summary>
        /// Tính parabol theo trục x
        /// </summary>
        public static Vector3 ParabolaX(Vector3 start, Vector3 end, float height, float t)
        {
            if (t >= 1) t = 1;
            Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }

        /// <summary>
        /// Tính parabol theo trục y
        /// </summary>
        public static Vector3 ParabolaY(Vector3 start, Vector3 end, float height, float t)
        {
            if (t >= 1) t = 1;
            Func<float, float> f = y => -4 * height * y * y + 4 * height * y;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(f(t) + Mathf.Lerp(start.x, end.x, t), mid.y, mid.z);
        }
    }
}
