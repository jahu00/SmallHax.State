using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine.Demo
{
    public interface IObjectWithState
    {
        Dictionary<string,Type> StateTypes { get; set; }
        IStateScript CurrentStateScript { get; set; }
        string CurrentState { get; set; }
    }

    public static class IObjectWithStateExtensions
    {
        public static void SetState(this IObjectWithState obj, string newState, object paramObj = null)
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

        public static void Process(this IObjectWithState obj)
        {
            obj.CurrentStateScript.Process();
        }
    }

}
