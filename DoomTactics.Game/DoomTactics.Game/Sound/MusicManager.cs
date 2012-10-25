using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace DoomTactics
{
    public static class MusicManager
    {
        private static Song _currentSong;
        private static bool _doPlayMusic = false;
        private static ContentManager _contentManager;

        public static void Initialize(ContentManager contentManager)
        {
            _contentManager = contentManager;
        }

        public static void PlayMusic(string songname, bool repeat)
        {
            if (_doPlayMusic)
            {
                _currentSong = _contentManager.Load<Song>(songname);
                MediaPlayer.IsRepeating = repeat;
                MediaPlayer.Volume = 0.6f;
                MediaPlayer.Play(_currentSong);
            }
        }

        internal static void PauseMusic()
        {
            MediaPlayer.Pause();
        }

        internal static void UnpauseMusic()
        {
            MediaPlayer.Resume();
        }

        internal static void StopMusic()
        {
            MediaPlayer.Stop();
            if (_currentSong != null && !_currentSong.IsDisposed)
            {
                _currentSong.Dispose();
            }
        }    

    }
}
