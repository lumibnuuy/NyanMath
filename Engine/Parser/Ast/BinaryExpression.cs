namespace NyanMath.Engine.Parser.Ast
{
    internal class BinaryExpression : OperatorExpression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public override void Accept(Evaluator evaluator)
        {
            evaluator.Visit(this);
        }
    }
}