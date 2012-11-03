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
        private static bool _doPlaySounds;
        private static AudioEngine _audioEngine;
        private static SoundBank _soundBank;
        private static WaveBank _waveBank;

        public static void Initialize(ContentManager contentManager)
        {
            _audioEngine = new AudioEngine("Sound/DoomTacticsAudio.xgs");
            _soundBank = new SoundBank(_audioEngine, "Sound/Sound Bank.xsb");
            _waveBank = new WaveBank(_audioEngine, "Sound/Wave Bank.xwb");
        }

        public static void PlaySound(string soundEffectName)
        {
            if (!_doPlaySounds)
                return;

            _soundBank.PlayCue(soundEffectName);            
        }

        public static void DisposeAll()
        {
            _waveBank.Dispose();
            _soundBank.Dispose();
            _audioEngine.Dispose();            
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
