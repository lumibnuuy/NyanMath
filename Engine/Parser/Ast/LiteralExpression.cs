namespace NyanMath.Engine.Parser.Ast
{
    internal class LiteralExpression : Expression
    {
        public float Value { get; private set; }

        public LiteralExpression(float value)
        {
            Value = value;
        }

        public override void Accept(Evaluator evaluator)
        {
            evaluator.Visit(this);
        }
    }
}
