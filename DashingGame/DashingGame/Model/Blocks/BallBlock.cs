using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashingGame.Model
{
    class BallBlock : BaseBlock
    {
        protected override GameTexture BlockTexture
        {
            get
            {
                return GameTexture.Ball;
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
    }
}
