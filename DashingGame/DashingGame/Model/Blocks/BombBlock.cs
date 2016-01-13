using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashingGame.Model
{
    class BombBlock : BaseBlock
    {
        public bool MustExplode = false;

        protected override GameTexture BlockTexture
        {
            get
            {
                return GameTexture.Bomb;
            }
        }

        public override bool doFall
        {
            get
            {
                return true;
            }
        }

        public override bool blockCanBePushed
        {
            get
            {
                return true;
            }
        }

        
        public void Explode()
        {
            this.ExplodeNeighbour(Directions.None);
            this.ExplodeNeighbour(Directions.South);
            this.ExplodeNeighbour(Directions.East);
            this.ExplodeNeighbour(Directions.West);
            this.ExplodeNeighbour(Directions.North);
        }

        public override bool GeneratePhysics()
        {
            base.GeneratePhysics();

            if (this.MustExplode || (this.isFalling && this.GetNeighbour(Directions.South) != null &&
                   this.GetNeighbour(Directions.South).triggersExplosion))
            {
                this.Explode();
            }

            return false;
        }
    }
}
