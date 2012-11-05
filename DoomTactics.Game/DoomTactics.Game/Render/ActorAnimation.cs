using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class ActorAnimation
    {
        private TimeSpan _animationTime;
        private readonly List<AnimationEntry> _animations;
        private readonly string _entityName;
        private readonly bool _loop;
        private bool _endEventsInvoked;
        private int _currentIndex;
        private Action _onComplete;

        public Action OnComplete
        {
            set { _onComplete = value; }
        }

        public ActorAnimation(bool loop, string entityName) : this(null, loop, entityName)
        {
            
        }

        public ActorAnimation(List<AnimationEntry> animationEntries, bool loop, string entityName)
        {
            _loop = loop;
            _entityName = entityName;
            _animations = animationEntries ?? new List<AnimationEntry>();
            _endEventsInvoked = false;
        }
        
        public ActorAnimation(bool loop, params AnimationEntry[] animationEntries)
        {
            _loop = loop;
            _entityName = "Prototype";
            _animations = animationEntries.ToList();
            _endEventsInvoked = false;
        }

        public void AddAnimationEntry(AnimationEntry animationEntry)
        {
            _animations.Add(animationEntry);
        }

        public string CurrentImageName()
        {
            return _animations[_currentIndex].SpriteName;
        }

        public void Update(TimeSpan elapsedTime)
        {
            _animationTime += elapsedTime;
            if (_animations.ElementAt(_currentIndex).Duration != TimeSpan.Zero && _animationTime >= _animations.ElementAt(_currentIndex).Duration)
            {
                TimeSpan difference = _animationTime - _animations.ElementAt(_currentIndex).Duration;
                _animationTime = difference;
                if (_currentIndex <= _animations.Count - 2)
                {
                    _currentIndex++;
                }
                else if (_animations.Count == _currentIndex + 1 && _loop)
                {
                    _currentIndex = 0;
                }
                else if (!_endEventsInvoked)
                {
                    _endEventsInvoked = true;
                    if (_onComplete != null)
                        _onComplete();
                    var evt = new DoomEvent(DoomEventType.AnimationEnd);
                    MessagingSystem.DispatchEvent(evt, _entityName);                    
                }
            }
        }

        public void Reset()
        {
            _animationTime = TimeSpan.Zero;
            _currentIndex = 0;
        }

        public ActorAnimation MakeCopy(string entityName)
        {
            var clone = new ActorAnimation(_loop, entityName);
            _animations.ForEach((i) => clone.AddAnimationEntry(new AnimationEntry(i)));
            clone.Reset();
            return clone;
        }
    }

    public struct AnimationEntry
    {
        public string SpriteName;
        public TimeSpan Duration;

        public AnimationEntry(string spriteName, TimeSpan duration)
        {
            SpriteName = spriteName;
            Duration = duration;
        }

        public AnimationEntry(AnimationEntry other)
        {
            SpriteName = other.SpriteName;
            Duration = other.Duration;
        }
    }
}
