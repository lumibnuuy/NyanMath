using System;
using System.Linq;
using NyanMath.Engine.Parser.Ast;

namespace NyanMath.Engine.Parser
{
    internal class Parser : IDisposable
    {
        private readonly Lexer _lex;
        private Token _current = null;
        private int _brackets = 0;

        public Parser(Lexer lex)
        {
            _lex = lex;
        }

        public void Dispose()
        {
            _lex?.Dispose();
        }

        public SyntaxTree Parse()
        {
            var root = ParseExpression();
            if (_brackets != 0)
            {
                throw new ParseException(_lex.Position, "Mismatched parentheses");
            }

            return new SyntaxTree(root);
        }

        private Expression ParseExpression(bool insideFunc = false)
        {
            if (!insideFunc)
            {
                Expect(InitialGroup);
            }
            else
            {
                Ensure(InitialGroupWithComma);
            }

            var expr = ParseAdditiveBinary();

            return expr;
        }

        private Expression ParseAdditiveBinary()
        {
            var expr = ParseMultiplicativeBinary();

            while (!_lex.IsEof() && (_current.Type == TokenType.Plus || _current.Type == TokenType.Minus))
            {
                var binaryExpr = new BinaryExpression
                {
                    Left = expr,
                    Operator = ((PunctuationToken) _current).ToOperatorType()
                };

                Expect(MiddleGroupAdditiveRelational);
                binaryExpr.Right = ParseMultiplicativeBinary();

                expr = binaryExpr;
            }

            return expr;
        }

        private Expression ParseMultiplicativeBinary()
        {
            var expr = ParseIdentifier();

            while (!_lex.IsEof() && (_current.Type == TokenType.Multiply || _current.Type == TokenType.Divide ||
                                     _current.Type == TokenType.Modulo || _current.Type == TokenType.Exponent))
            {
                var binaryExpr = new BinaryExpression
                {
                    Left = expr,
                    Operator = ((PunctuationToken) _current).ToOperatorType()
                };

                Expect(MiddleGroupMultiplicative);
                binaryExpr.Right = ParseIdentifier();

                expr = binaryExpr;
            }

            return expr;
        }

        private Expression ParseIdentifier()
        {
            if (_current == null)
            {
                throw new ParseException(_lex.Position, "Expected function or expression");
            }

            if (_current.Type != TokenType.Identifier)
            {
                return ParseUnary();
            }

            var identifier = (IdentifierToken) _current;

            var peek = _lex.PeekToken();
            if (peek == null || peek.Type != TokenType.LeftParen)
            {
                // variable
                var varExpr = new VariableExpression(identifier.AsString);

                if (_lex.PeekToken() != null)
                {
                    Expect(MiddleGroupIdentifier);
                }
                else
                {
                    Consume();
                }

                return varExpr;
            }

            // function call
            var expr = new FunctionCallExpression(identifier.AsString);
            Expect(TokenType.LeftParen);

            while (!_lex.IsEof())
            {
                Consume();
                if (_current.Type == TokenType.RightParen)
                {
                    Consume();
                    break;
                }

                expr.Arguments.Add(ParseExpression(true));

                if (_current == null)
                {
                    break;
                }

                if (_current.Type == TokenType.Comma)
                {
                    continue;
                }

                if (_current.Type == TokenType.RightParen)
                {
                    Consume();
                    break;
                }

                throw new ParseException(_lex.Position, "Expected comma or closing parenthesis");
            }

            return expr;
        }

        private Expression ParseUnary()
        {
            if (_current == null)
            {
                throw new ParseException(_lex.Position, "Expected unary operator, literal, or opening parenthesis");
            }

            if (_current.Type == TokenType.Literal)
            {
                var literal = ParseLiteral();
                Consume();
                return literal;
            }

            var unary = new UnaryExpression();
            if (_current.Type == TokenType.Minus)
            {
                unary.Operator = OperatorType.UnaryMinus;
                Expect(MiddleGroupUnary);
            }
            else if (_current.Type == TokenType.Plus)
            {
                unary.Operator = OperatorType.UnaryPlus;
                Expect(MiddleGroupUnary);
            }

            if (_current.Type == TokenType.Literal)
            {
                unary.Value = ParseLiteral();
            }
            else if (_current.Type == TokenType.LeftParen)
            {
                unary.Value = ParseExpression();
            }
            else if (_current.Type == TokenType.Identifier)
            {
                unary.Value = ParseIdentifier();
            }
            else
            {
                throw new ParseException(_lex.Position, "Expected literal or opening parenthesis");
            }

            Consume();
            return unary;
        }

        private LiteralExpression ParseLiteral()
        {
            var literal = new LiteralExpression(((LiteralToken) _current).AsFloat);
            if (_lex.PeekToken() != null && _lex.PeekToken().Type == TokenType.Literal)
            {
                throw new ParseException(_lex.Position, "Expected expression");
            }

            return literal;
        }

        private void Ensure(TokenType[] types, bool allowIdent = true)
        {
            if (_current == null)
            {
                throw new ParseException(_lex.Position, "Unexpected end of input");
            }

            if (!((allowIdent && _current.Type == TokenType.Identifier) || types.Contains(_current.Type)))
            {
                throw new ParseException(_lex.Position, $"Unexpected token: {_current.Value}");
            }
        }

        private void Expect(TokenType type)
        {
            var next = _lex.PeekToken();
            if (next == null)
            {
                throw new ParseException(_lex.Position, $"Unexpected end of input");
            }

            if (next.Type == type)
            {
                Consume();
            }
            else
            {
                throw new ParseException(_lex.Position, $"Unexpected token: {next.Value}");
            }
        }

        private void Expect(TokenType[] types, bool allowIdent = true)
        {
            var next = _lex.PeekToken();
            if (next == null)
            {
                throw new ParseException(_lex.Position, $"Unexpected end of input");
            }

            if ((allowIdent && next.Type == TokenType.Identifier) || types.Contains(next.Type))
            {
                Consume();
            }
            else
            {
                throw new ParseException(_lex.Position, $"Unexpected token: {next.Value}");
            }
        }

        private void Consume()
        {
            _current = _lex.NextToken();
            if (_current == null)
            {
                return;
            }

            if (_current.Type == TokenType.LeftParen)
            {
                _brackets++;
            }
            else if (_current.Type == TokenType.RightParen)
            {
                _brackets--;
            }
        }

        // all groups also implicitly allow identifiers where appropriate
        private static readonly TokenType[] InitialGroup =
        {
            TokenType.Literal,
            TokenType.Plus,
            TokenType.Minus,
            TokenType.LeftParen
        };

        private static readonly TokenType[] InitialGroupWithComma =
        {
            TokenType.Literal,
            TokenType.Plus,
            TokenType.Minus,
            TokenType.LeftParen,
            TokenType.Comma
        };

        private static readonly TokenType[] MiddleGroupAdditiveRelational =
        {
            TokenType.Literal,
            TokenType.Plus,
            TokenType.Minus,
            TokenType.LeftParen,
            TokenType.RightParen
        };

        private static readonly TokenType[] MiddleGroupMultiplicative =
        {
            TokenType.Literal,
            TokenType.Plus,
            TokenType.Minus,
            TokenType.LeftParen,
        };

        private static readonly TokenType[] MiddleGroupIdentifier =
        {
            TokenType.Literal,
            TokenType.Plus,
            TokenType.Minus,
            TokenType.Multiply,
            TokenType.Divide,
            TokenType.Modulo,
            TokenType.Exponent,
            TokenType.LeftParen,
            TokenType.RightParen,
            TokenType.Comma
        };

        private static readonly TokenType[] MiddleGroupUnary =
        {
            TokenType.Literal,
            TokenType.LeftParen
        };
    }
}
