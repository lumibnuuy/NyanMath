using System;
using System.Globalization;
using System.Text;
using NyanMath.Engine.Parser.Ast;

namespace NyanMath.Engine.Parser
{
    internal class Lexer : IDisposable
    {
        private Tokenizer _tok;
        private Token _current;
        private Token _next;

        public int Position => _tok.Position;

        public Lexer(Tokenizer tok)
        {
            _tok = tok;
            _next = Lex();
        }

        public Token NextToken()
        {
            _current = _next;
            _next = Lex();
            return _current;
        }

        public Token PeekToken() => _next;

        public bool IsEof() => _next == null;

        private Token Lex()
        {
            SkipWhitespace();

            var c = _tok.NextChar();
            var buf = new StringBuilder();

            switch (c)
            {
                case '+':
                    return new PunctuationToken(TokenType.Plus);
                case '-':
                    return new PunctuationToken(TokenType.Minus);
                case '*':
                    return new PunctuationToken(TokenType.Multiply);
                case '/':
                    return new PunctuationToken(TokenType.Divide);
                case '%':
                    return new PunctuationToken(TokenType.Modulo);
                case '^':
                    return new PunctuationToken(TokenType.Exponent);
                case '(':
                    return new PunctuationToken(TokenType.LeftParen);
                case ')':
                    return new PunctuationToken(TokenType.RightParen);
                case ',':
                    return new PunctuationToken(TokenType.Comma);
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    var seenDot = c == '.';

                    buf.Clear();
                    buf.Append(c);

                    // Main literal parsing loop
                    while (true)
                    {
                        c = _tok.PeekChar();
                        if (c == '.') // Decimals
                        {
                            if (seenDot)
                            {
                                throw new ParseException(_tok.Position, "Unexpected '.' in numeric literal");
                            }

                            seenDot = true;
                        }
                        else if (c == 'e' || c == 'E') // Exponent notation
                        {
                            buf.Append(c);
                            _tok.NextChar();

                            c = _tok.PeekChar();
                            if (c == '+' || c == '-' || IsDigit(c))
                            {
                                buf.Append(c);
                                _tok.NextChar();

                                while (IsDigit(c = _tok.PeekChar()))
                                {
                                    buf.Append(c);
                                    _tok.NextChar();
                                }

                                return new LiteralToken(ParseNumber(buf, seenDot, true));
                            }
                        }
                        else if (!IsDigit(c))
                        {
                            break;
                        }

                        buf.Append(c);
                        _tok.NextChar();
                    }

                    return new LiteralToken(ParseNumber(buf, seenDot, false));
                case '.':
                    if (IsDigit(_tok.PeekChar()))
                    {
                        goto case '9';
                    }

                    throw new ParseException(_tok.Position, "Invalid numeric literal");
                case '\0': // EOF
                    return null;
                default:
                    if (IsValidInIdentifier(c))
                    {
                        buf.Clear();
                        buf.Append(c);

                        while (true)
                        {
                            c = _tok.PeekChar();
                            if (!IsValidInIdentifier(c))
                            {
                                break;
                            }

                            buf.Append(c);
                            _tok.NextChar();
                        }

                        var ident = buf.ToString();
                        return new IdentifierToken(ident);
                    }

                    throw new ParseException(_tok.Position, $"Unexpected character: {c}");
            }
        }

        private float ParseNumber(StringBuilder buf, bool seenDot, bool seenExp)
        {
            var styles = NumberStyles.None;
            if (seenDot)
            {
                styles |= NumberStyles.AllowDecimalPoint;
            }

            if (seenExp)
            {
                styles |= NumberStyles.AllowExponent;
            }

            return float.Parse(buf.ToString(), styles, CultureInfo.InvariantCulture);
        }

        private static bool IsValidInIdentifier(char c)
        {
            return char.IsLetterOrDigit(c) || c == '_';
        }

        private static bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private void SkipWhitespace()
        {
            while (_tok.PeekChar() == ' ')
            {
                _tok.NextChar();
            }
        }

        public void Dispose()
        {
            _tok?.Dispose();
        }
    }
}
