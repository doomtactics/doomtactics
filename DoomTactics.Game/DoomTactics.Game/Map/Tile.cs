using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics.Map
{
    public struct TileTextures
    {
        public Texture2D Top;
        public Texture2D North;
        public Texture2D South;
        public Texture2D East;
        public Texture2D West;

        /// <summary>
        /// Constructor for tiles that have only ONE texture on all sides
        /// </summary>
        /// <param name="texture"></param>
        public TileTextures(Texture2D texture) : this(texture, texture, texture, texture, texture) { }

        public TileTextures(Texture2D top, Texture2D sides) : this(top, sides, sides, sides, sides) { }

        public TileTextures(Texture2D top, Texture2D north, Texture2D south, Texture2D east, Texture2D west)
        {
            Top = top;
            North = north;
            South = south;
            East = east;
            West = west;
        }
    }

    public class Tile
    {
        private const float TileLength = 64.0f;
        private const int NumVertexes = 30;

        public int XCoord;
        public int YCoord;
        public decimal Height;
        private readonly Texture2D _texture;
        private VertexPositionNormalTexture[] _vertexes;
        private TileTextures _tileTextures;

        public Tile(TileTextures tileTextures, Vector3 position, float height)
        {
            _tileTextures = tileTextures;
            Construct(position, height);
        }

        public void Render(GraphicsDevice device, BasicEffect effect)
        {
            using (var buffer = new VertexBuffer(
                device,
                VertexPositionNormalTexture.VertexDeclaration,
                NumVertexes,
                BufferUsage.WriteOnly))
            {

                // Load the buffer
                buffer.SetData(_vertexes);

                // Send the vertex buffer to the device
                device.SetVertexBuffer(buffer);

                // top
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    effect.Texture = _tileTextures.Top;
                    pass.Apply();
                    device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
                }

                // north
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    effect.Texture = _tileTextures.North;
                    pass.Apply();
                    device.DrawPrimitives(PrimitiveType.TriangleList, 6, 2);
                }
                
                // south
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    effect.Texture = _tileTextures.South;
                    pass.Apply();
                    device.DrawPrimitives(PrimitiveType.TriangleList, 12, 2);
                }
                
                // east
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    effect.Texture = _tileTextures.East;
                    pass.Apply();
                    device.DrawPrimitives(PrimitiveType.TriangleList, 18, 2);
                }

                // west
                foreach (EffectPass pass in effect.CurrentTechnique.Passes)
                {
                    effect.Texture = _tileTextures.West;
                    pass.Apply();
                    device.DrawPrimitives(PrimitiveType.TriangleList, 24, 2);
                }
            }
        }

        private void Construct(Vector3 position, float height)
        {
            _vertexes = new VertexPositionNormalTexture[NumVertexes];
            Vector3 normalTop = Vector3.Up;
            /*Vector3 topLeftFront = position;
            Vector3 topRightFront = position + Vector3.Backward + Vector3.Right;
            Vector3 topRightBack = position + Vector3.Right;
            Vector3 topLeftBack = position + Vector3.Backward;*/
            Vector3 topLeftFront = position + new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 topLeftBack = position + new Vector3(0.0f, 0.0f, TileLength);
            Vector3 topRightFront = position + new Vector3(TileLength, 0.0f, 0.0f);
            Vector3 topRightBack = position + new Vector3(TileLength, 0.0f, TileLength);
            Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
            Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);
            Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);

            Vector3 bottomLeftFront = position + new Vector3(0.0f, -height, 0.0f);
            Vector3 bottomRightFront = position + new Vector3(TileLength, -height, 0.0f);
            Vector3 bottomLeftBack = position + new Vector3(0.0f, -height, TileLength);
            Vector3 bottomRightBack = position + new Vector3(TileLength, -height, TileLength);

            /* TOP FACE */
            _vertexes[0] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureTopLeft);
            _vertexes[1] = new VertexPositionNormalTexture(topRightBack, normalTop, textureBottomRight);
            _vertexes[2] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureBottomLeft);
            _vertexes[3] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureTopLeft);
            _vertexes[4] = new VertexPositionNormalTexture(topRightFront, normalTop, textureTopRight);
            _vertexes[5] = new VertexPositionNormalTexture(topRightBack, normalTop, textureBottomRight);

            /* NORTH FACE */
            _vertexes[6] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureTopLeft);
            _vertexes[7] = new VertexPositionNormalTexture(bottomRightFront, normalTop, textureBottomRight);
            _vertexes[8] = new VertexPositionNormalTexture(topRightFront, normalTop, textureTopRight);
            _vertexes[9] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureTopLeft);
            _vertexes[10] = new VertexPositionNormalTexture(bottomLeftFront, normalTop, textureBottomLeft);
            _vertexes[11] = new VertexPositionNormalTexture(bottomRightFront, normalTop, textureBottomRight);

            /* SOUTH FACE */
            _vertexes[12] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopLeft);
            _vertexes[13] = new VertexPositionNormalTexture(bottomRightBack, normalTop, textureBottomRight);
            _vertexes[14] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);
            _vertexes[15] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopLeft);
            _vertexes[16] = new VertexPositionNormalTexture(bottomLeftBack, normalTop, textureBottomLeft);
            _vertexes[17] = new VertexPositionNormalTexture(bottomRightBack, normalTop, textureBottomRight);

            /* EAST FACE */
            _vertexes[18] = new VertexPositionNormalTexture(topRightFront, normalTop, textureTopLeft);
            _vertexes[19] = new VertexPositionNormalTexture(bottomRightBack, normalTop, textureBottomRight);
            _vertexes[20] = new VertexPositionNormalTexture(topRightBack, normalTop, textureTopRight);
            _vertexes[21] = new VertexPositionNormalTexture(topRightFront, normalTop, textureTopLeft);
            _vertexes[22] = new VertexPositionNormalTexture(bottomRightFront, normalTop, textureBottomLeft);
            _vertexes[23] = new VertexPositionNormalTexture(bottomRightBack, normalTop, textureBottomRight);

            /* WEST FACE */
            _vertexes[24] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureTopLeft);
            _vertexes[25] = new VertexPositionNormalTexture(bottomLeftBack, normalTop, textureBottomRight);
            _vertexes[26] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureTopRight);
            _vertexes[27] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureTopLeft);
            _vertexes[28] = new VertexPositionNormalTexture(bottomLeftFront, normalTop, textureBottomLeft);
            _vertexes[29] = new VertexPositionNormalTexture(bottomLeftBack, normalTop, textureBottomRight);
        }
    }
}
