using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.State
{
    public abstract class StateScript<TOwner, TStateKey> : IStateScript<TOwner, TStateKey> where TStateKey : Enum
    {
        protected TOwner Owner { get; set; }
        protected StateMachine<TOwner,TStateKey> StateMachine { get; set; }

        public virtual void Deinitialize()
        {
        }

        public virtual void Initialize(TOwner owner, StateMachine<TOwner, TStateKey> stateMachine, object stateArgs = null)
        {
            Owner = owner;
            StateMachine = stateMachine;
        }

        public abstract void Process();
    }
}
