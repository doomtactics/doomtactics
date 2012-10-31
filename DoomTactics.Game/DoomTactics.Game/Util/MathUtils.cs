using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NLog;

namespace DoomTactics
{
    public static class MathUtils
    {
        public static readonly Logger Logger = LogManager.GetCurrentClassLogger();        

        public static float SignedAngleOnXzPlane(Vector3 vec1, Vector3 vec2)
        {
            var v1 = new Vector2(vec1.X, vec1.Z);
            var v2 = new Vector2(vec2.X, vec2.Z);
            v1.Normalize();
            v2.Normalize();
            float angle = (float)Math.Acos(Vector2.Dot(v2, v1));
            //Logger.Trace(v1 + ", " + v2 + ", " + angle);
            if (Vector3.Cross(vec1, vec2).Y < 0.0f)
            {
                return angle;
            }
            else
            {
                return angle + MathHelper.Pi;
            }
        }

        public static int DistanceBetweenTiles(Tile tile1, Tile tile2)
        {
            return Math.Abs(tile1.XCoord - tile2.XCoord) + Math.Abs(tile1.YCoord - tile2.YCoord);
        }
    }
}
