namespace NyanMath.Engine.Parser.Ast
{
    internal abstract class Expression
    {
        public abstract void Accept(Evaluator evaluator);
    }
}
