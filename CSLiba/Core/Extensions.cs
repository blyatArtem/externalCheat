using CSLiba.Imports;
using Microsoft.Xna.Framework;
using System;
using System.Windows.Forms;

namespace CSLiba.Core
{
    public static class Extensions
    {
        private const double _PI_Over_180 = Math.PI / 180.0;

        private const double _180_Over_PI = 180.0 / Math.PI;

        public static double DegreeToRadian(this double degree)
        {
            return degree * _PI_Over_180;
        }

        public static double RadianToDegree(this double radian)
        {
            return radian * _180_Over_PI;
        }

        public static float DegreeToRadian(this float degree)
        {
            return (float)(degree * _PI_Over_180);
        }

        public static float RadianToDegree(this float radian)
        {
            return (float)(radian * _180_Over_PI);
        }
        public static bool IsValidScreen(this Vector3 value)
        {
            return !value.X.IsInfinityOrNaN() && !value.Y.IsInfinityOrNaN() && value.Z >= 0 && value.Z < 1;
        }
        public static bool IsInfinityOrNaN(this float value)
        {
            return float.IsNaN(value) || float.IsInfinity(value);
        }

        public static bool Down(this Keys key)
        {
            return (User32.GetAsyncKeyState(key) & 0x8000) != 0;
        }
        public static float Dot(this Vector3 left, Vector3 right)
        {
            return Vector3.Dot(left, right);
        }

        public static bool IsZero(this GameOverlay.Drawing.Rectangle rect)
        {
            return rect.Left == 0 && rect.Top == 0 && rect.Right == 0 && rect.Bottom == 0;
        }
        public static Vector3 Normalized(this Vector3 value)
        {
            return Vector3.Normalize(value);
        }

        public static bool IsParallelTo(this Vector3 vector, Vector3 other, float tolerance = 1E-6f)
        {
            return Math.Abs(1.0 - Math.Abs(vector.Normalized().Dot(other.Normalized()))) <= tolerance;
        }

        public static System.Drawing.Color ToColor(this Vector4 vec)
        {
            if (vec.X < 0)
                return System.Drawing.Color.Black;
            return System.Drawing.Color.FromArgb((int)vec.X, (int)vec.Y, (int)vec.Z);
        }

        public static Matrix ToMatrix(this in Game.Structs.matrix3x4_t matrix)
        {
            return new Matrix
            {
                M11 = matrix.m00,
                M12 = matrix.m01,
                M13 = matrix.m02,

                M21 = matrix.m10,
                M22 = matrix.m11,
                M23 = matrix.m12,

                M31 = matrix.m20,
                M32 = matrix.m21,
                M33 = matrix.m22,

                M41 = matrix.m30,
                M42 = matrix.m31,
                M43 = matrix.m32,
                M44 = 1,
            };
        }
    }
}
