using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DoomTactics
{
    public class ActorAnimation : ICloneable
    {
        private TimeSpan _animationTime;
        private readonly List<AnimationEntry> _animations;
        private int _currentIndex;
        private bool _loop;

        public ActorAnimation(bool loop) : this(null, loop)
        {
            
        }

        public ActorAnimation(List<AnimationEntry> animationEntries, bool loop)
        {
            _loop = loop;
            _animations = animationEntries ?? new List<AnimationEntry>();
        }

        public void AddAnimationEntry(AnimationEntry animationEntry)
        {
            _animations.Add(animationEntry);
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
                else
                {
                    MessagingSystem.DispatchEvent();
                }
            }
        }

        public void Reset()
        {
            _animationTime = TimeSpan.Zero;
            _currentIndex = 0;
        }

        public object Clone()
        {
            var clone = new ActorAnimation(_loop);
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
