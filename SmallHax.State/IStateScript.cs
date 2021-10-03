using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.State
{
    public interface IStateScript<TOwner,TStateKey> where TStateKey : Enum
    {
        void Initialize(TOwner owner, StateMachine<TOwner,TStateKey> stateMachine, object stateArgs = null);
        void Process();
        void Deinitialize();

    }
}
