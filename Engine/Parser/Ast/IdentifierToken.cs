namespace NyanMath.Engine.Parser.Ast
{
    internal class IdentifierToken : Token
    {
        public IdentifierToken(string value) : base(TokenType.Identifier, value)
        {
        }

        public string AsString => (string) Value;
    }
}
