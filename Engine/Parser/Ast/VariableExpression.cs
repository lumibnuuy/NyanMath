namespace NyanMath.Engine.Parser.Ast
{
    internal class VariableExpression : NameExpression
    {
        public VariableExpression(string name)
        {
            Name = name;
        }

        public override void Accept(Evaluator evaluator)
        {
            evaluator.Visit(this);
        }
    }
}