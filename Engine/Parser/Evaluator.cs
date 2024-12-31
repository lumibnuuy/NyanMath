using System;
using NyanMath.Engine.Parser.Ast;

namespace NyanMath.Engine.Parser
{
    internal class Evaluator
    {
        public float Result { get; private set; }

        public void Visit(LiteralExpression expr)
        {
            Result = expr.Value;
        }

        public void Visit(UnaryExpression expr)
        {
            expr.Value.Accept(this);

            switch (expr.Operator)
            {
                case OperatorType.UnaryPlus:
                    Result = 0f + Result;
                    break;
                case OperatorType.UnaryMinus:
                    Result = 0f - Result;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Visit(BinaryExpression expr)
        {
            expr.Left.Accept(this);
            var left = Result;
            expr.Right.Accept(this);
            var right = Result;

            switch (expr.Operator)
            {
                case OperatorType.Add:
                    Result = left + right;
                    break;
                case OperatorType.Subtract:
                    Result = left - right;
                    break;
                case OperatorType.Multiply:
                    Result = left * right;
                    break;
                case OperatorType.Divide:
                    Result = left / right;
                    break;
                case OperatorType.Modulo:
                    Result = left % right;
                    break;
                case OperatorType.Power:
                    Result = (float) Math.Pow(left, right);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Visit(VariableExpression expr)
        {
            switch (expr.Name)
            {
                case "e":
                    Result = (float) Math.E;
                    break;
                case "pi":
                    Result = (float) Math.PI;
                    break;
                default:
                    Result = VNyanInterface.VNyanInterface.VNyanParameter.getVNyanParameterFloat(expr.Name);
                    break;
            }
        }

        public void Visit(FunctionCallExpression expr)
        {
            var name = expr.Name;
            var def = BuiltinFunctions.Find(name);
            if (def == null)
            {
                throw new ParseException($"Unknown function {name}");
            }

            var args = new float[expr.Arguments.Count];
            for (var i = 0; i < expr.Arguments.Count; i++)
            {
                expr.Arguments[i].Accept(this);
                args[i] = Result;
            }

            Result = def.Delegate(args);
        }

        public static float Evaluate(string expression)
        {
            var tree = SyntaxTree.FromString(expression);
            var visitor = new Evaluator();
            tree.Root.Accept(visitor);
            return visitor.Result;
        }
    }
}
