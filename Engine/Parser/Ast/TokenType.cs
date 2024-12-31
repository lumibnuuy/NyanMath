namespace NyanMath.Engine.Parser.Ast
{
    public enum TokenType
    {
        // Literal numbers
        Literal,

        // Function or variable
        Identifier,

        // commas separate function parameters
        Comma,

        // Arithmetic operators
        Plus,
        Minus,
        Multiply,
        Divide,
        Modulo,
        Exponent,

        // Parentheses for scope control
        LeftParen,
        RightParen,
    }
}