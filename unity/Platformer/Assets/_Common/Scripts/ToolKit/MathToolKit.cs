///-----------------------------------------------------------------
/// Author : Hugo TEYSSIER
/// Date : 10/11/2019 16:52
///-----------------------------------------------------------------

using UnityEngine;

namespace Com.IsartDigital.Common.ToolKit {

	public static class MathToolKit {

        public static Vector3 SphereCoordinate(float radius, float hAngle, float vAngle)
        {
            return new Vector3(radius * Mathf.Cos(vAngle) * Mathf.Cos(hAngle),
                                radius * Mathf.Sin(vAngle),
                                radius * Mathf.Cos(vAngle) * Mathf.Sin(hAngle));
        }

        public static float Loop(float value, float min, float max)
        {
            if (value > max) return min;
            if (value < min) return max;
            return value;
        }

        public static float RecursiveLoop(float value, float min, float max)
        {
            if (value > max) return RecursiveLoop(value - max + min, min, max);
            if (value < min) return RecursiveLoop(value + max - min, min, max);
            return value;
        }

        public static bool IsClose(float a, float b, float tolerance)
        {
            return a >= b - tolerance && a <= b + tolerance;
        }

        public static Vector2 RandomOnUnitSquare()
        {
            int randomInt = RandomBool() ? 1 : -1;
            float randomRange = Random.Range(-1f, 1f);

            return RandomBool() ? new Vector2(randomRange, randomInt) : new Vector2(randomInt, randomRange);
        }

        public static Vector2 RandomOnUnitRect(Rect rect)
        {
            float x;
            float y;

            if (RandomBool())
            {
                x = RandomBool() ? rect.x : rect.xMax;
                y = Random.Range(rect.y, rect.yMax);
            }
            else
            {
                x = Random.Range(rect.x, rect.xMax);
                y = RandomBool() ? rect.y : rect.yMax;
            }

            return new Vector2(x, y);
        }

        public static bool RandomBool()
        {
            return Random.value > 0.5f;
        }
    }
}