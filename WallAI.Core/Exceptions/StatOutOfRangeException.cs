using System;

namespace WallAI.Core.Exceptions
{
    public class StatOutOfRangeException<T> : Exception
    {
        public StatOutOfRangeException(string stat, T statValue, T maxStatValue) :
            base($"MaxStats value for {stat} ({maxStatValue}) is smaller than Stats value ({statValue}).")
        { }
    }
}
