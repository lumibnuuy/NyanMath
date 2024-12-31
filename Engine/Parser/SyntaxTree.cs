using NyanMath.Engine.Parser.Ast;

namespace NyanMath.Engine.Parser
{
    internal class SyntaxTree
    {
        public Expression Root { get; private set; }

        public SyntaxTree(Expression root)
        {
            Root = root;
        }

        public static SyntaxTree FromString(string expr)
        {
            using (var lex = new Lexer(new Tokenizer(expr)))
            {
                return new Parser(lex).Parse();
            }
        }
    }
}