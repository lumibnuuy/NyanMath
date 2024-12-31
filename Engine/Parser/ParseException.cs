using System;

namespace NyanMath.Engine.Parser
{
    [Serializable]
    public class ParseException : Exception
    {
        public int Position { get; private set; }

        public ParseException(string message) : base(message)
        {
            Position = -1;
        }

        public ParseException(int position, string message) : base(message)
        {
            Position = position;
        }

        public override string ToString()
        {
            return Position != -1
                ? $"Expression error at position {Position}: {Message}"
                : $"Expression error: {Message}";
        }
    }
}