using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NLog;

namespace DoomTactics
{
    public abstract class ActorBase
    {
        protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public string ActorID;
        public int Height;
        public int Width;
        public int Speed;
        public int ChargeTime;
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
            ChargeTime = 0;
        }

        public virtual void Update(GameTime elapsedTime)
        {
            CurrentAnimation.Update(elapsedTime.ElapsedGameTime);
            ChargeTime += Speed;
        }

        public void Render(GraphicsDevice device, SpriteBatch spriteBatch, AlphaTestEffect spriteEffect, Camera camera, int passNumber)
        {
            Matrix bill = Matrix.CreateConstrainedBillboard(Position, camera.Position, Vector3.Down, camera.Direction,
                                                            Vector3.Forward);

            spriteEffect.World = bill;
            spriteEffect.View = camera.View;
            spriteEffect.Projection = camera.Projection;

            SpriteRenderDetails details = SpriteSheet.GetRectangle(CurrentAnimation.CurrentImageName(), GetAngle(camera));

            if (passNumber == 0)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.DepthRead,
                                  RasterizerState.CullNone, spriteEffect);
                device.DepthStencilState = DepthStencilState.Default;
                spriteBatch.Draw(SpriteSheet.Texture, new Rectangle(-Width / 2, -Height, Width, Height), details.Rectangle, Color.White, 0.0f, Vector2.Zero, details.SpriteEffects, 0);
                spriteBatch.End();
            }
            else
            {
                // spritebatch begin ignores depth buffer
                spriteBatch.Begin(0, null, SamplerState.PointClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, spriteEffect);
                spriteBatch.Draw(SpriteSheet.Texture, new Rectangle(-Width / 2, -Height, Width, Height), details.Rectangle, Color.White, 0.0f, Vector2.Zero, details.SpriteEffects, 0);
                spriteBatch.End();
            }
        }

        private Angle GetAngle(Camera camera)
        {
            Vector3 vectorBetween = camera.Position - Position;
            float angle = MathHelper.ToDegrees(MathUtils.SignedAngleOnXzPlane(vectorBetween, Vector3.Forward));
            Angle angleEnum;

            if (angle > -22.5 && angle <= 22.5)
            {
                angleEnum = Angle.Right;
            }
            else if (angle > 22.5 && angle <= 67.5)
            {
                angleEnum = Angle.ForwardRight;
            }
            else if (angle > 67.5 && angle <= 112.5)
            {
                angleEnum = Angle.Forward;
            }
            else if (angle > 112.5 && angle <= 157.5)
            {
                angleEnum = Angle.ForwardLeft;
            }
            else if (angle > 157.5 || angle <= -157.5)
            {
                angleEnum = Angle.Left;
            }
            else if (angle > -157.5 && angle <= -112.5)
            {
                angleEnum = Angle.BackLeft;
            }
            else if (angle > -112.5 && angle <= -67.5)
            {
                angleEnum = Angle.Back;
            }
            else
            {
                angleEnum = Angle.BackRight;
            }
            /*
            if (ActorID == "Imp1")
            {
                Logger.Trace("Camera: " + camera.Position + ", my pos: " + Position + "Between: " + vectorBetween +
                             ", angle: " + angle);
                Logger.Trace("Angle: " + angleEnum.ToString());
            }
             */
            return angleEnum;
        }


        public BoundingBox CreateBoundingBox()
        {
            var bb = new BoundingBox(new Vector3(Position.X - Width/2, Position.Y, Position.Z - Width/2),
                                   new Vector3(Position.X + Width/2, Position.Y + Height, Position.Z + Width/2));
            return bb;
        }
    }
}
