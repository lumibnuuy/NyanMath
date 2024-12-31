using System.Collections.Generic;

namespace NyanMath.Engine.Parser.Ast
{
    internal class FunctionCallExpression : NameExpression
    {
        public List<Expression> Arguments { get; private set; }

        public FunctionCallExpression(string name) : base()
        {
            Name = name;
            Arguments = new List<Expression>();
        }

        public override void Accept(Evaluator evaluator)
        {
            evaluator.Visit(this);
        }
    }
}