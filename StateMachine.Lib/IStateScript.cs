using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StateMachine
{
    public interface IStateScript
    {
        void Initialize(object owner, object paramObj = null);
        void Process();
        void Deinitialize();

    }
}
