using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashingGame.Model
{
    class PlayerBlock : BaseBlock
    {
        protected override GameTexture BlockTexture
        {
            get
            {
                return GameTexture.Player;
            }
        }
        public void UpdatePosition(Board board, Directions Direction)
        {
            BaseBlock block = this.GetNeighbour(Direction);

            if (block == null || block.blockCanBeConsumed)
            {
                this.MoveTo(Direction);
            }
            if (block != null && block.blockCanBeConsumed)
            {
                if (block is HeartBlock)
                {
                   
                    board.HeartsTaken++;
                    Sounds.Instance.PlayEffect(SoundType.Heart);
                }
                if (block is ExitBlock)
                {
                    Sounds.Instance.PlayEffect(SoundType.LevelFinish);
                }
                if (!(block is HeartBlock))
                {
                    Sounds.Instance.PlayEffect(SoundType.SnowShuffeld);
                }
                board.RemoveBlock(block);
            }

            if (block != null && block.blockCanBePushed && ((Direction == Directions.East && block.GetNeighbour(Directions.East) == null)
                 ||(Direction == Directions.West && block.GetNeighbour(Directions.West) == null )))
            {
                this.MoveTo(Direction);
                block.MoveTo(Direction);
            }
        }
    }
}
