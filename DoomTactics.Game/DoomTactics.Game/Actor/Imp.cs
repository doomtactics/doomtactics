using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class Imp : ActorBase
    {
        public Texture2D tex;
        private static readonly Rectangle texrec = new Rectangle(0, 0, 40, 56);
        private VertexPositionNormalTexture[] _vertexes;

        public Imp(string id, Vector3 position, Texture2D impTex)
            : base(id)
        {
            Height = 70;
            Width = 50;
            Position = position;
            tex = impTex;
        }

        public void Render(GraphicsDevice device, SpriteBatch spriteBatch, BasicEffect spriteEffect, Camera camera)
        {
            Matrix bill = Matrix.CreateConstrainedBillboard(Position, camera.Position, Vector3.Down, camera.Direction,
                                                            Vector3.Forward);

            spriteEffect.World = bill;
            spriteEffect.View = camera.View;
            spriteEffect.Projection = camera.Projection;

            spriteEffect.TextureEnabled = true;
            spriteEffect.VertexColorEnabled = true;

            spriteBatch.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, spriteEffect);
            spriteBatch.Draw(tex, new Rectangle(-Width / 2, -Height, Width, Height), texrec, Color.White);
            spriteBatch.End();
        }
    }
}
