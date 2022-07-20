namespace Game.Signals
{
    public class GameSignals
    {
        //Pool
        public readonly Signal<bool> InitPool = new Signal<bool>();

        //GameStart
        public readonly Signal GameStart = new Signal();
    }
}
