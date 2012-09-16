using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace DoomTactics.Map
{
    public class Tile
    {
        private const float TileLength = 64.0f;

        public int XCoord;
        public int YCoord;
        public decimal Height;
        private readonly Texture2D _texture;
        private VertexPositionNormalTexture[] _vertexes;        

        public Tile(Texture2D texture, Vector3 position)
        {
            _texture = texture;
            Construct(position);
        }

        public void Render(GraphicsDevice device)
        {
            using (var buffer = new VertexBuffer(
               device,
               VertexPositionNormalTexture.VertexDeclaration,
               36,
               BufferUsage.WriteOnly))
            {
                // Load the buffer
                buffer.SetData(_vertexes);

                // Send the vertex buffer to the device
                device.SetVertexBuffer(buffer);
            }

            // Draw the primitives from the vertex buffer to the device as triangles
            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);  
        }

        private void Construct(Vector3 position)
        {
            _vertexes = new VertexPositionNormalTexture[6];
            Vector3 normal = Vector3.Up;
            /*Vector3 topLeftFront = position;
            Vector3 topRightFront = position + Vector3.Backward + Vector3.Right;
            Vector3 topRightBack = position + Vector3.Right;
            Vector3 topLeftBack = position + Vector3.Backward;*/
            Vector3 topLeftFront = position + new Vector3(0.0f, 1.0f, 0.0f);
            Vector3 topLeftBack = position + new Vector3(0.0f, 1.0f, TileLength);
            Vector3 topRightFront = position + new Vector3(TileLength, 1.0f, 0.0f);
            Vector3 topRightBack = position + new Vector3(TileLength, 1.0f, TileLength);
            Vector2 textureTopLeft = new Vector2(1.0f, 0.0f);
            Vector2 textureTopRight = new Vector2(0.0f, 0.0f);
            Vector2 textureBottomLeft = new Vector2(1.0f, 1.0f);
            Vector2 textureBottomRight = new Vector2(0.0f, 1.0f);

            _vertexes[0] = new VertexPositionNormalTexture(topLeftFront, normal, textureBottomLeft);
            _vertexes[1] = new VertexPositionNormalTexture(topRightBack, normal, textureTopRight);
            _vertexes[2] = new VertexPositionNormalTexture(topLeftBack, normal, textureTopLeft);
            _vertexes[3] = new VertexPositionNormalTexture(topLeftFront, normal, textureBottomLeft);
            _vertexes[4] = new VertexPositionNormalTexture(topRightFront, normal, textureBottomRight);
            _vertexes[5] = new VertexPositionNormalTexture(topRightBack, normal, textureTopRight);
        }
    }
}
