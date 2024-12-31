namespace NyanMath.Engine.Parser.Ast
{
    internal abstract class OperatorExpression : Expression
    {
        public OperatorType Operator { get; set; }
    }
}
