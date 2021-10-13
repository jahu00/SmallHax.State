using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.State
{
    public delegate void StateChangeEventHandler<TOwner, TStateKey>(object sender, StateChangeEventArgs<TOwner,TStateKey> stateChangeEventArgs) where TStateKey : Enum;

    public class StateMachine<TOwner,TStateKey> where TStateKey : Enum
    {
        private TOwner Owner { get; set; }
        private Dictionary<TStateKey, Type> StateScripts { get; set; } = new Dictionary<TStateKey, Type>();
        public IStateScript<TOwner,TStateKey> CurrentStateScript { get; private set; } = null;
        public TStateKey CurrentState { get; private set; } = (TStateKey)(object)-1;

        public event StateChangeEventHandler<TOwner, TStateKey> StateChanging;
        public event StateChangeEventHandler<TOwner, TStateKey> StateChanged;
        public StateMachine(TOwner owner)
        {
            Owner = owner;
        }

        public void SetState(TStateKey newState, object stateArgs = null)
        {
            var oldState = CurrentState;
            var stateChangeEventArs = new StateChangeEventArgs<TOwner, TStateKey>()
            {
                Owner = Owner,
                NewState = newState,
                OldState = oldState,
                StateArgs = stateArgs
            };

            StateChanging?.Invoke(this, stateChangeEventArs);

            if (CurrentStateScript != null)
            {
                CurrentStateScript.Deinitialize();
            }

            var newStateScriptType = StateScripts[newState];
            var newStateScript = (IStateScript<TOwner,TStateKey>)Activator.CreateInstance(newStateScriptType);
            CurrentState = newState;
            CurrentStateScript = newStateScript;
            CurrentStateScript.Initialize(Owner, this, stateArgs);

            StateChanged?.Invoke(this, stateChangeEventArs);
        }

        public void Process()
        {
            CurrentStateScript.Process();
        }

        public void AddState<TStateScript>(TStateKey state) where TStateScript : IStateScript<TOwner, TStateKey>
        {
            var stateScriptType = typeof(TStateScript);
            StateScripts.Add(state, stateScriptType);
        }
    }
}
