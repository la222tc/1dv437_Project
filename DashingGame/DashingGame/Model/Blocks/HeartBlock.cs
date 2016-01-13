using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashingGame.Model
{
    class HeartBlock : BaseBlock
    {
        protected override GameTexture BlockTexture
        {
            get
            {
                return GameTexture.Heart;
            }
        }

        public override bool blockCanBeConsumed
        {
            get
            {
                return true;
            }
        }

        public override bool doFall
        {
            get
            {
                return true;
            }
        }
    }
}
