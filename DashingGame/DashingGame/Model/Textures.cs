using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DashingGame.Model
{
    public enum GameTexture { Empty, Snow, Wall, Ball, Heart, Player, Exit, Bomb, Boom};

    public class Textures
    {
        Dictionary<GameTexture, Texture2D> _textures = new Dictionary<GameTexture,Texture2D>();

        private GraphicsDevice _device;
        public Textures(GraphicsDevice Device)
        {
            _device = Device;
            _instance = this;
        }

        private static Textures _instance;
        public static Textures Instance
        {
            get
            {
                return _instance;
            }
        }

        public Texture2D GetTexture(GameTexture Texture)
        {
            if (!_textures.ContainsKey(Texture))
                _textures.Add(Texture, Texture2D.FromStream(_device, File.Open((string.Format("Content\\{0}.png", Texture)), FileMode.Open)));

            return _textures[Texture];
        }
    }
}
