using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace DashingGame.Model
{
    public enum SoundType { Heart, LevelFinish, BombExplodes, SnowShuffeld };
    class Sounds
    {
        Dictionary<SoundType, SoundEffect> _effects = new Dictionary<SoundType, SoundEffect>();
        private static Sounds _instance;
        public static Sounds Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Sounds();

                return _instance;
            }
        }

        public Sounds()
        {
            foreach (SoundType value in Enum.GetValues(typeof(SoundType)))
            {
                GetSoundEffect(value);
            }
        }
        
        private SoundEffect GetSoundEffect(SoundType Type)
        {
            if (!_effects.ContainsKey(Type))
                using (FileStream fs = File.Open((string.Format("Content\\{0}.wav", Type)), FileMode.Open))
                {
                    _effects.Add(Type, SoundEffect.FromStream(fs));
                }
            return _effects[Type];
        }

        public void PlayEffect(SoundType Type)
        {
            SoundEffect sound = GetSoundEffect(Type);
            if (sound != null)
            {
                sound.Play();
            }
        }
    }
}

