using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public abstract class ActorBase
    {
        public string ActorID;
        public int Height;
        public int Width;

        public Vector3 Position;

        public ActorBase(string id)
        {
            ActorID = id;
        }
    }
}
