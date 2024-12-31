namespace NyanMath.Engine.Parser.Ast
{
    internal class UnaryExpression : OperatorExpression
    {
        public Expression Value { get; set; }

        public override void Accept(Evaluator evaluator)
        {
            evaluator.Visit(this);
        }
    }
}