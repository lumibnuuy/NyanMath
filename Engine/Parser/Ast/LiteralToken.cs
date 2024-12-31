namespace NyanMath.Engine.Parser.Ast
{
    internal class LiteralToken : Token
    {
        public LiteralToken(float value) : base(TokenType.Literal, value)
        {
        }

        public float AsFloat => (float) Value;
    }
}
