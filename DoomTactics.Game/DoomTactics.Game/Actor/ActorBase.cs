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

        public void Render(GraphicsDevice device, SpriteBatch spriteBatch, BasicEffect spriteEffect, Camera camera)
        {
            Matrix bill = Matrix.CreateConstrainedBillboard(Position, camera.Position, Vector3.Down, camera.Direction,
                                                            Vector3.Forward);

            spriteEffect.World = bill;
            spriteEffect.View = camera.View;
            spriteEffect.Projection = camera.Projection;

            spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, spriteEffect);
            device.DepthStencilState = DepthStencilState.Default;            
            device.BlendState = BlendState.AlphaBlend;
            device.RasterizerState = RasterizerState.CullNone;
            spriteBatch.Draw(MyTexture, new Rectangle(-Width / 2, -Height, Width, Height), MyRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
