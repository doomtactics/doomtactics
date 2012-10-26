using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace DoomTactics
{
    public static class SoundManager
    {
        private static readonly IList<SoundEffect> SoundEffects = new List<SoundEffect>();
        private static ContentManager _contentManager;
        private static bool _doPlaySounds;

        public static void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public static void PlaySound(string soundEffectName)
        {
            if (!_doPlaySounds)
                return;

            if (!SoundEffects.Any(se => se.Name == soundEffectName))
            {
                var se = _contentManager.Load<SoundEffect>(soundEffectName);
                se.Name = soundEffectName;
                SoundEffects.Add(se);
            }
            var effect = (SoundEffects.FirstOrDefault(se => se.Name == soundEffectName));
            var effectInstance = effect.CreateInstance();
            effectInstance.IsLooped = false;
            effectInstance.Play();
        }

        public static void DisposeAll()
        {
            foreach (var se in SoundEffects)
            {
                if (!se.IsDisposed)
                {
                    se.Dispose();
                }
            }

        }

        public static void OnPlaySound(IDoomEvent soundEvent)
        {
            var evt = (SoundEvent) soundEvent;
            PlaySound(evt.SoundName);
        }

        public static void SetPlaySound(bool enabled)
        {
            _doPlaySounds = enabled;
        }
    }
}
