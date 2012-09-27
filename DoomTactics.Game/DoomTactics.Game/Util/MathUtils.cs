using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public static class MathUtils
    {
        public static float SignedAngleOnXzPlane(Vector3 vec1, Vector3 vec2)
        {
            var v1 = new Vector2(vec1.X, vec1.Z);
            var v2 = new Vector2(vec2.X, vec2.Z);
            v1.Normalize();
            v2.Normalize();
            float angle = (float)Math.Acos(Vector2.Dot(v2, v1));
            if (Vector3.Cross(new Vector3(vec1.X, 0, vec1.Z), new Vector3(vec2.X, 0, vec2.Z)).Y < 0.0f)
            {
                return angle;
            }
            else
            {
                return -angle;
            }
        }
    }
}
