using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.State
{
    public class StateChangeEventArgs<TOwner, TStateKey>
    {
        public TOwner Owner { get; set; }
        public TStateKey OldState { get; set; }
        public TStateKey NewState { get; set; }
        public object StateArgs { get; set; }
    }
}
