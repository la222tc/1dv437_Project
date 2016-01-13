using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashingGame.Model
{
    class WallBlock : BaseBlock
    {
        protected override GameTexture BlockTexture
        {
            get
            {
                return GameTexture.Wall;
            }
        }
        public override bool othersFallFrom
        {
            get
            {
                return true;
            }
        }
    }
}
