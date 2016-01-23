using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.IO;

namespace DashingGame.Model
{
    public class Board
    {
        public const int BOARDSIZEX = 20;
        public const int BOARDSIZEY = 12;
        public const int BLOCKSIZE = 40;

        public string LevelName { get; set; }
        public int HeartsToComplete { get; set; }
        public int HeartsTaken { get; set; }

        List<BaseBlock> _blocks = new List<BaseBlock>();

        public Board()
        {
            HeartsToComplete = 0;
        }

        public bool Restart
        {
            get
            {
                return this.Blocks.All(b => !(b is PlayerBlock) && !(b is BoomBlock));
            }
        }

        public bool CompletedLevel
        {
            get
            {
                return this.Blocks.All(b => !(b is ExitBlock));
            }
        }
        public IEnumerable<BaseBlock> Blocks
        {
            get
            {
                return _blocks;
            }
        }
        public void AddBlock(BaseBlock Block)
        {
            Block._board = this;

            if (Block is HeartBlock)
            {
                HeartsToComplete++;
            }
             _blocks.Add(Block);
        }


        public void DrawBoard(SpriteBatch spriteBatch)
        {
            foreach (var block in Blocks)
            {
                spriteBatch.Draw( block.Texture, new Rectangle( block.X * BLOCKSIZE, block.Y * BLOCKSIZE, BLOCKSIZE, BLOCKSIZE ), Color.White );
            }
        }
       

        public bool UpdateBoard(GameTime gameTime, KeyboardState state)
        {
            foreach (var block in Blocks)
            {
                block.playerMoved = false;
            }
            foreach (int row in Enumerable.Range(0, BOARDSIZEY).Reverse())
            {
                foreach (int col in Enumerable.Range(0, BOARDSIZEX))
                {
                    var Block = Blocks.Where(b => !b.playerMoved && b.doFall && b.Y == row && b.X == col).FirstOrDefault();
                    if (Block != null)
                    {
                        Block.GeneratePhysics();
                    }
                }
            }
            return true;
        }

        public bool UpdatePlayer(Directions playerDirection)
        {
            PlayerBlock player = Blocks.OfType<PlayerBlock>().FirstOrDefault();

            if (player != null)
            {
                player.UpdatePosition(this, playerDirection);
            }
            if (HeartsToComplete == HeartsTaken)
            {
                this.Blocks.OfType<ExitBlock>().ToList().ForEach(e => e.levelOpen());
            }

            return true;
        }
        public void KillThePlayer()
        {
            PlayerBlock player = Blocks.OfType<PlayerBlock>().FirstOrDefault();

            if (player != null)
                player.ExplodeNeighbour(Directions.None);
        }

        public void RemoveBlock(BaseBlock block)
        {
            _blocks.Remove(block);
        }

    }
}
