using System;
using System.IO;

namespace NyanMath.Engine.Parser
{
    public class Tokenizer : IDisposable
    {
        private TextReader _reader;
        private int _index;

        public Tokenizer(string input)
        {
            _reader = new StringReader(input);
            _index = 0;
        }

        public int Position => _index;

        public char NextChar()
        {
            var c = _reader.Read();
            if (c == -1)
            {
                return '\0';
            }

            _index++;
            return (char) c;
        }

        public char PeekChar()
        {
            var c = _reader.Peek();
            if (c == -1)
            {
                return '\0';
            }

            return (char) c;
        }

        public void Dispose()
        {
            _reader?.Dispose();
        }
    }
}
