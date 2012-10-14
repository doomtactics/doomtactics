using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics
{
    public class TileModel
    {
        private const float TileLength = 64.0f;
        private const int NumVertexes = 30;
        private VertexPositionNormalTexture[] _vertexes;
        private TileTextures _tileTextures;
        private BoundingBox _box;

        public TileModel(TileTextures tileTextures, Vector3 position, float height)
        {
            _tileTextures = tileTextures;
            Construct(position, height);
        }

        private void Construct(Vector3 position, float visualHeight)
        {
            _vertexes = new VertexPositionNormalTexture[NumVertexes];
            Vector3 normalTop = Vector3.Up;
            Vector3 normalNorth = Vector3.Forward;
            Vector3 normalSouth = Vector3.Backward;
            Vector3 normalEast = Vector3.Right;
            Vector3 normalWest = Vector3.Left;
            Vector3 topLeftFront = position + new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 topLeftBack = position + new Vector3(0.0f, 0.0f, TileLength);
            Vector3 topRightFront = position + new Vector3(TileLength, 0.0f, 0.0f);
            Vector3 topRightBack = position + new Vector3(TileLength, 0.0f, TileLength);
            Vector2 textureTopRight = new Vector2(1.0f, 0.0f);
            Vector2 textureTopLeft = new Vector2(0.0f, 0.0f);
            Vector2 textureBottomRight = new Vector2(1.0f, 1.0f);
            Vector2 textureBottomLeft = new Vector2(0.0f, 1.0f);

            Vector3 bottomLeftFront = position + new Vector3(0.0f, -visualHeight, 0.0f);
            Vector3 bottomRightFront = position + new Vector3(TileLength, -visualHeight, 0.0f);
            Vector3 bottomLeftBack = position + new Vector3(0.0f, -visualHeight, TileLength);
            Vector3 bottomRightBack = position + new Vector3(TileLength, -visualHeight, TileLength);

            /* TOP FACE */
            _vertexes[0] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureTopLeft);
            _vertexes[1] = new VertexPositionNormalTexture(topRightBack, normalTop, textureBottomRight);
            _vertexes[2] = new VertexPositionNormalTexture(topLeftBack, normalTop, textureBottomLeft);
            _vertexes[3] = new VertexPositionNormalTexture(topLeftFront, normalTop, textureTopLeft);
            _vertexes[4] = new VertexPositionNormalTexture(topRightFront, normalTop, textureTopRight);
            _vertexes[5] = new VertexPositionNormalTexture(topRightBack, normalTop, textureBottomRight);

            /* NORTH FACE */
            _vertexes[6] = new VertexPositionNormalTexture(topLeftFront, normalNorth, textureTopRight);
            _vertexes[7] = new VertexPositionNormalTexture(bottomRightFront, normalNorth, textureBottomLeft);
            _vertexes[8] = new VertexPositionNormalTexture(topRightFront, normalNorth, textureTopLeft);
            _vertexes[9] = new VertexPositionNormalTexture(topLeftFront, normalNorth, textureTopRight);
            _vertexes[10] = new VertexPositionNormalTexture(bottomLeftFront, normalNorth, textureBottomRight);
            _vertexes[11] = new VertexPositionNormalTexture(bottomRightFront, normalNorth, textureBottomLeft);

            /* SOUTH FACE */
            _vertexes[12] = new VertexPositionNormalTexture(topLeftBack, normalSouth, textureTopLeft);
            _vertexes[13] = new VertexPositionNormalTexture(bottomRightBack, normalSouth, textureBottomRight);
            _vertexes[14] = new VertexPositionNormalTexture(topRightBack, normalSouth, textureTopRight);
            _vertexes[15] = new VertexPositionNormalTexture(topLeftBack, normalSouth, textureTopLeft);
            _vertexes[16] = new VertexPositionNormalTexture(bottomLeftBack, normalSouth, textureBottomLeft);
            _vertexes[17] = new VertexPositionNormalTexture(bottomRightBack, normalSouth, textureBottomRight);

            /* EAST FACE */
            _vertexes[18] = new VertexPositionNormalTexture(topRightFront, normalEast, textureTopRight);
            _vertexes[19] = new VertexPositionNormalTexture(bottomRightBack, normalEast, textureBottomLeft);
            _vertexes[20] = new VertexPositionNormalTexture(topRightBack, normalEast, textureTopLeft);
            _vertexes[21] = new VertexPositionNormalTexture(topRightFront, normalEast, textureTopRight);
            _vertexes[22] = new VertexPositionNormalTexture(bottomRightFront, normalEast, textureBottomRight);
            _vertexes[23] = new VertexPositionNormalTexture(bottomRightBack, normalEast, textureBottomLeft);

            /* WEST FACE */
            _vertexes[24] = new VertexPositionNormalTexture(topLeftFront, normalWest, textureTopLeft);
            _vertexes[25] = new VertexPositionNormalTexture(bottomLeftBack, normalWest, textureBottomRight);
            _vertexes[26] = new VertexPositionNormalTexture(topLeftBack, normalWest, textureTopRight);
            _vertexes[27] = new VertexPositionNormalTexture(topLeftFront, normalWest, textureTopLeft);
            _vertexes[28] = new VertexPositionNormalTexture(bottomLeftFront, normalWest, textureBottomLeft);
            _vertexes[29] = new VertexPositionNormalTexture(bottomLeftBack, normalWest, textureBottomRight);

            
            _box = new BoundingBox(new Vector3(position.X, position.Y - visualHeight, position.Z), new Vector3(position.X + TileLength, position.Y, position.Z + TileLength));
        }

        public void Render(GraphicsDevice device, BasicEffect effect, Effect highlightEffect, bool isHighlighted)
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

                if (isHighlighted)
                {
                    foreach (EffectPass pass in highlightEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
                    }
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

        public BoundingBox CreateBoundingBox()
        {
            return _box;
        }
    }

    public class Tile
    {
        public int XCoord;
        public int YCoord;
        private readonly TileModel _model;
        public ActorBase ActorInTile { get; private set; }
        public decimal GameHeight { get; private set; }

        public Tile(TileTextures tileTextures, Vector3 position, float visualHeight, int xcoord, int ycoord, decimal gameHeight)
        {
            _model = new TileModel(tileTextures, position, visualHeight);
            XCoord = xcoord;
            YCoord = ycoord;
        }

        public void SetActor(ActorBase actorBase)
        {
            ActorInTile = actorBase;
        }

        public void Render(GraphicsDevice device, BasicEffect effect, Effect highlightEffect, bool isHighlighted)
        {
            _model.Render(device, effect, highlightEffect, isHighlighted);         
        }

        public BoundingBox CreateBoundingBox()
        {
            return _model.CreateBoundingBox();
        }

    }
}
