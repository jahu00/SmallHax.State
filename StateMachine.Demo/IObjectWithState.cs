using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine.Demo
{
    public interface IObjectWithState
    {

    }

    public interface IObjectWithState<TStateKey> : IObjectWithState
    {
        Dictionary<TStateKey, Type> StateTypes { get; set; }
        IStateScript CurrentStateScript { get; set; }
        TStateKey CurrentState { get; set; }
    }

    public static class IObjectWithStateExtensions
    {
        public static void SetState<TStateKey>(this IObjectWithState<TStateKey> obj, TStateKey newState, object paramObj = null)
        {
            if (obj.CurrentStateScript != null)
            {
                obj.CurrentStateScript.Deinitialize();
            }

            var newStateType = obj.StateTypes[newState];
            var newStateScript = (IStateScript)Activator.CreateInstance(newStateType);
            obj.CurrentState = newState;
            obj.CurrentStateScript = newStateScript;
            obj.CurrentStateScript.Initialize(obj, paramObj);
        }

        public static void Process<TStateKey>(this IObjectWithState<TStateKey> obj)
        {
            obj.CurrentStateScript.Process();
        }

        public static void AddState<TStateScript>(this IObjectWithState obj, object stateKey) where TStateScript : IStateScript
        {
            var type = obj.GetType();
            var stateTypesProperty = type.GetProperty("StateTypes");
            var stateTypes = (IDictionary)stateTypesProperty.GetValue(obj);
            stateTypes.Add(stateKey, typeof(TStateScript));
        }
    }

}
