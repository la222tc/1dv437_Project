using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DashingGame.Model
{
    public enum Directions { None, NorthWest, North, NorthEast, West, East, SouthWest, South, SouthEast }
    public class BaseBlock
    {
        public int X;   // = rowNumber
        public int Y;   // = colNumber

        public bool playerMoved = false;
        private bool _blockCanBeConsumed = false;
        protected bool isFalling = false;
        public Board _board { get; set; }

        protected virtual GameTexture BlockTexture
        {
            get
            {
                return GameTexture.Empty;
            }
        }
        public void levelOpen()
        {
            _blockCanBeConsumed = true;
        }
        public virtual Texture2D Texture
        {
            get
            {
                return Textures.Instance.GetTexture(BlockTexture);
            }
        }
        public void MoveTo(Directions Direction)
        {
            X += GetNeighbourDeltaX(Direction);
            Y += GetNeighbourDeltaY(Direction);
            playerMoved = true;
        }
        public BaseBlock GetNeighbour(Directions Direction)
        {
            return GetNeighbour(GetNeighbourDeltaX(Direction), GetNeighbourDeltaY(Direction));
        }

        public BaseBlock GetNeighbour(int deltaX, int deltaY)
        {
            int NewX = X + deltaX;
            int NewY = Y + deltaY;

            if (NewX < 0 || NewX > Board.BOARDSIZEX - 1 || NewY < 0 || NewY > Board.BOARDSIZEY - 1)
            {
                return new WallBlock();
            }

                return _board.Blocks.Where(b => b.X == NewX &&b.Y == NewY).FirstOrDefault();
        }
        private int GetNeighbourDeltaY(Directions Direction)
        {
            switch (Direction)
            {
                case Directions.NorthWest:
                case Directions.North:
                case Directions.NorthEast:
                    return -1;

                case Directions.SouthWest:
                case Directions.South:
                case Directions.SouthEast:
                    return +1;

                default:
                    return 0;
            }
        }

        private int GetNeighbourDeltaX(Directions Direction)
        {
            switch (Direction)
            {
                case Directions.NorthWest:
                case Directions.West:
                case Directions.SouthWest:
                    return -1;

                case Directions.NorthEast:
                case Directions.East:
                case Directions.SouthEast:
                    return +1;

                default:
                    return 0;
            }
        }
        public void ExplodeNeighbour(Directions Direction)
        {
            Sounds.Instance.PlayEffect(SoundType.BombExplodes);
            BaseBlock neighbour = this.GetNeighbour(Direction);
            if (neighbour != null && neighbour.blockCanExplode)
            {
                if (neighbour is BombBlock && (Direction == Directions.West || Direction == Directions.East || Direction == Directions.South || Direction == Directions.North))
                {
                    BombBlock bombNeighbour = neighbour as BombBlock;
                    bombNeighbour.MustExplode = true;
                }
                else
                {
                    this._board.RemoveBlock(neighbour);
                    this._board.AddBlock(new BoomBlock() { X = neighbour.X, Y = neighbour.Y });
                }
            }
            if (neighbour == null)
            {
                this._board.AddBlock(new BoomBlock() { X = this.X + this.GetNeighbourDeltaX(Direction), Y = this.Y + this.GetNeighbourDeltaY(Direction)});
            }

        }

        public virtual bool blockCanBeConsumed
        {
            get
            {
                return _blockCanBeConsumed;
            }
        }
        public virtual bool doFall
        {
            get
            {
                return false;
            }
        }
        public virtual bool blockCanBePushed
        {
            get
            {
                return false;
            }
        }
        public virtual bool blockCanExplode
        {
            get
            {
                return true;
            }
        }

        public virtual bool triggersExplosion
        {
            get
            {
                return true;
            }
        }

        public virtual bool othersFallFrom
        {
            get
            {
                return false;
            }
        }


        public virtual bool GeneratePhysics()
        {
            BaseBlock block = this.GetNeighbour(Directions.South);
            if (block == null)
            {
                this.MoveTo(Directions.South);
                isFalling = true;

                return false;
            }
            else
            {
                if (block is PlayerBlock && this.isFalling)
                {
                    block.ExplodeNeighbour(Directions.None);
                    //_board.GetBombSound().Play();
                }

                if ((block.doFall && !block.isFalling) || (block.othersFallFrom))
                {
                    if (this.GetNeighbour(Directions.East) == null && this.GetNeighbour(Directions.SouthEast) == null &&
                         (this.GetNeighbour(Directions.NorthEast) == null || !this.GetNeighbour(Directions.NorthEast).doFall))
                    {
                        this.MoveTo(Directions.East);
                        isFalling = true;

                        return false;
                    }
                    if (this.GetNeighbour(Directions.West) == null && this.GetNeighbour(Directions.SouthWest) == null &&
                         (this.GetNeighbour(Directions.NorthWest) == null || !this.GetNeighbour(Directions.NorthWest).doFall))
                    {
                        this.MoveTo(Directions.West);
                        isFalling = true;

                        return false;
                    }
                }
            }

            isFalling = false;
            return true;
        }
    }
}
