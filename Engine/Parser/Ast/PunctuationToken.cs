namespace NyanMath.Engine.Parser.Ast
{
    internal class PunctuationToken : Token
    {
        public PunctuationToken(TokenType type) : base(type, null)
        {
        }

        public OperatorType ToOperatorType()
        {
            switch (Type)
            {
                case TokenType.Plus:
                    return OperatorType.Add;
                case TokenType.Minus:
                    return OperatorType.Subtract;
                case TokenType.Multiply:
                    return OperatorType.Multiply;
                case TokenType.Divide:
                    return OperatorType.Divide;
                case TokenType.Modulo:
                    return OperatorType.Modulo;
                case TokenType.Exponent:
                    return OperatorType.Power;
                default:
                    throw new ParseException("Unexpected punctuation token");
            }
        }
    }
}
