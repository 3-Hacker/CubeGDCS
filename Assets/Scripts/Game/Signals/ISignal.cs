using System;

namespace Game.Signals
{
    public interface ISignal
    {
        Delegate Listener { get; set; }
        void RemoveAllListeners();
    }
}
