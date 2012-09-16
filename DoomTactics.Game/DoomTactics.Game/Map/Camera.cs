using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class Camera
    {
        private const float PanModifier = 0.5f;
        private const float FreelookSpeed = 2.0f;

        public Vector3 Position;
        public Vector3 Direction;
        public Vector3 Up;
        public Matrix View;
        public Matrix Projection;
        private readonly string _cameraName;
        private const float ZNear = 1.0f;
        private const float ZFar = 1000.0f;
        private static readonly float Fov = MathHelper.ToRadians(45);

        public Camera(string cameraName, Vector3 position, Vector3 target, Vector3 up, float aspectRatio)
        {
            Position = position;
            Direction = (target - position);
            Direction.Normalize();
            Up = up;
            UpdateView();
            Projection = Matrix.CreatePerspectiveFieldOfView(Fov, aspectRatio, ZNear, ZFar);
        }

        public void MoveCamera(IDoomEvent evt)
        {
            var cameraEvent = (CameraEvent)evt;
            Vector3 sideVector = Vector3.Cross(Up, Direction);
            sideVector.Normalize();

            if (cameraEvent.CameraMovementMode == CameraMovementMode.Pan)
            {
                Position += sideVector * PanModifier * cameraEvent.MouseDelta.X;
                Position += Up * PanModifier * cameraEvent.MouseDelta.Y;
            }
            else
            {
                if (cameraEvent.ForwardPressed)
                    Position += FreelookSpeed * Direction;
                else if (cameraEvent.BackwardPressed)
                    Position -= FreelookSpeed * Direction;
                if (cameraEvent.StrafeLeftPressed)
                    Position += FreelookSpeed * sideVector;
                else if (cameraEvent.StrafeRightPressed)
                    Position -= FreelookSpeed * sideVector;

                if (cameraEvent.MouseDelta.X != 0)
                {
                    Direction = Vector3.Transform(Direction,
                                                  Matrix.CreateFromAxisAngle(Up,
                                                                             -MathHelper.PiOver4/360*
                                                                             cameraEvent.MouseDelta.X));
                }

                if (cameraEvent.MouseDelta.Y != 0)
                {
                    Direction = Vector3.Transform(Direction,
                                                  Matrix.CreateFromAxisAngle(sideVector,
                                                                             MathHelper.PiOver4/360*
                                                                             cameraEvent.MouseDelta.Y));
                }
            }
            UpdateView();
        }


        private void UpdateView()
        {
            View = Matrix.CreateLookAt(Position, Position + Direction, Up);
        }

    }
}
