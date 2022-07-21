using System;

namespace Game.Enums
{
    [Flags]
    public enum GameStatus
    {
        None = 1 << 0,
        Blocked = 1 << 1,
        UnBlocked = 1 << 2,
        Game = 1 << 3,
    }
}
