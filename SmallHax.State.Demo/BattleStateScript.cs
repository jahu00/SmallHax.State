using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.State.Demo
{
    public abstract class BattleStateScript : StateScript<Battle, BattleState>
    {
        protected Battle Battle => Owner;
    }
}
