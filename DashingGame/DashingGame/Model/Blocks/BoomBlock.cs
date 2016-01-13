using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashingGame.Model
{
    class BoomBlock : BaseBlock
    {
        const int MAXFRAMES = 3;
        int Frame = 0;
        protected override GameTexture BlockTexture
        {
            get
            {
                return GameTexture.Boom;
            }
        }

        public override bool blockCanExplode
        {
            get
            {
                return false;
            }
        }

        public override bool doFall
        {
            get
            {
                return true;
            }
        }

       
        public override bool GeneratePhysics()
        {
            if (Frame < MAXFRAMES)
                Frame++;
            else
            {
                Frame = 0;
                this._board.RemoveBlock(this);
            }

            return false;
        }
    }
}
