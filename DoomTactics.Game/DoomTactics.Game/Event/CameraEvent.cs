using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public enum CameraMovementMode
    {
        Freelook,
        Pan
    }

    public struct CameraEvent : IDoomEvent
    {
        private readonly DoomEventType _eventType;
        private readonly string[] _listenerNames;
        private readonly Vector2 _mouseDelta;
        private readonly bool _forwardPressed;
        private readonly bool _backwardPressed;
        private readonly bool _strafeRightPressed;
        private readonly bool _strafeLeftPressed;
        private readonly CameraMovementMode _cameraMovementMode;

        public CameraEvent(DoomEventType eventType, Vector2 mouseDelta, params string[] listenerNames)
        {
            _eventType = eventType;
            _mouseDelta = mouseDelta;
            _listenerNames = listenerNames;
            _forwardPressed = false;
            _backwardPressed = false;
            _strafeLeftPressed = false;
            _strafeRightPressed = false;
            _cameraMovementMode = CameraMovementMode.Pan;
        }

        public CameraEvent(DoomEventType eventType, Vector2 mouseDelta, bool forwardPressed, bool backwardPressed,
                           bool strafeRightPressed, bool strafeLeftPressed, params string[] listenerNames)
        {
            _eventType = eventType;
            _mouseDelta = mouseDelta;
            _listenerNames = listenerNames;
            _forwardPressed = forwardPressed;
            _backwardPressed = backwardPressed;
            _strafeLeftPressed = strafeLeftPressed;
            _strafeRightPressed = strafeRightPressed;
            _cameraMovementMode = CameraMovementMode.Freelook;            
        }

        public CameraMovementMode CameraMovementMode
        {
            get { return _cameraMovementMode; }
        }

        public bool StrafeLeftPressed
        {
            get { return _strafeLeftPressed; }
        }

        public bool StrafeRightPressed
        {
            get { return _strafeRightPressed; }
        }

        public bool BackwardPressed
        {
            get { return _backwardPressed; }
        }

        public bool ForwardPressed
        {
            get { return _forwardPressed; }
        }

        public DoomEventType EventType
        {
            get { return _eventType; }
        }

        public string[] ListenerNames
        {
            get { return _listenerNames; }
        }

        public Vector2 MouseDelta
        {
            get { return _mouseDelta; }
        }
    }
}
