using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashingGame.Model
{
    class SnowBlock : BaseBlock
    {
        protected override GameTexture BlockTexture
        {
            get
            {
                return GameTexture.Snow;
            }
        }
        public override bool blockCanBeConsumed
        {
            get
            {
                return true;
            }
        }
        public override bool triggersExplosion
        {
            get
            {
                return false;
            }
        }

    }
}
