using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public abstract class ActorBase
    {
        public string ActorID;
        public int Height;
        public int Width;

        public virtual Texture2D MyTexture 
        { 
            get;
            protected set;
        }

        public virtual Rectangle MyRectangle
        {
            get; 
            protected set; 
        } 
        public Vector3 Position;

        public ActorBase(string id)
        {
            ActorID = id;
        }

        public void Render(GraphicsDevice device, SpriteBatch spriteBatch, AlphaTestEffect spriteEffect, Camera camera, int passNumber)
        {
            Matrix bill = Matrix.CreateConstrainedBillboard(Position, camera.Position, Vector3.Down, camera.Direction,
                                                            Vector3.Forward);

            spriteEffect.World = bill;
            spriteEffect.View = camera.View;
            spriteEffect.Projection = camera.Projection;

            if (passNumber == 0)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, null, DepthStencilState.DepthRead,
                                  RasterizerState.CullNone, spriteEffect);
                device.DepthStencilState = DepthStencilState.Default;
                spriteBatch.Draw(MyTexture, new Rectangle(-Width/2, -Height, Width, Height), MyRectangle, Color.White);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, spriteEffect);
                spriteBatch.Draw(MyTexture, new Rectangle(-Width / 2, -Height, Width, Height), MyRectangle, Color.White);
                spriteBatch.End();
            }
        }
    }
}
