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
        protected ActorAnimation CurrentAnimation;        

        public virtual SpriteSheet SpriteSheet 
        { 
            get;
            protected set;
        }

        public virtual Rectangle CurrentTextureRectangle
        {
            get; 
            protected set;
        }

        public Vector3 Position;

        protected ActorBase(string id)
        {
            ActorID = id;
        }

        public virtual void Update(GameTime elapsedTime)
        {
            CurrentAnimation.Update(elapsedTime.ElapsedGameTime);
        }

        public void Render(GraphicsDevice device, SpriteBatch spriteBatch, AlphaTestEffect spriteEffect, Camera camera, int passNumber)
        {
            Matrix bill = Matrix.CreateConstrainedBillboard(Position, camera.Position, Vector3.Down, camera.Direction,
                                                            Vector3.Forward);

            spriteEffect.World = bill;
            spriteEffect.View = camera.View;
            spriteEffect.Projection = camera.Projection;

            Rectangle textureRectangle = SpriteSheet.GetRectangle(CurrentAnimation.CurrentImageName());

            if (passNumber == 0)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.DepthRead,
                                  RasterizerState.CullNone, spriteEffect);
                device.DepthStencilState = DepthStencilState.Default;
                spriteBatch.Draw(SpriteSheet.Texture, new Rectangle(-Width/2, -Height, Width, Height), textureRectangle, Color.White);
                spriteBatch.End();
            }
            else
            {
                // spritebatch begin ignores depth buffer
                spriteBatch.Begin(0, null, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, spriteEffect);
                spriteBatch.Draw(SpriteSheet.Texture, new Rectangle(-Width / 2, -Height, Width, Height), textureRectangle, Color.White);
                spriteBatch.End();
            }
        }
    }
}
