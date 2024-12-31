namespace NyanMath.Engine.Parser.Ast
{
    internal abstract class NameExpression : Expression
    {
        public string Name { get; protected set; }
    }
}