using System;
using System.Collections.Generic;

namespace NyanMath.Engine
{
    internal class BuiltinFunctions
    {
        private static Dictionary<string, FunctionDefinition>
            Definitions = new Dictionary<string, FunctionDefinition>();

        static BuiltinFunctions()
        {
            Definitions.Add("round", new FunctionDefinition("round", Round));
            Definitions.Add("ceil", new FunctionDefinition("ceil", Ceil));
            Definitions.Add("floor", new FunctionDefinition("floor", Floor));
            Definitions.Add("abs", new FunctionDefinition("abs", Abs));
            Definitions.Add("sqrt", new FunctionDefinition("sqrt", Sqrt));
            Definitions.Add("log", new FunctionDefinition("log", Log));
            Definitions.Add("min", new FunctionDefinition("min", Min));
            Definitions.Add("max", new FunctionDefinition("max", Max));
            Definitions.Add("pow", new FunctionDefinition("pow", Pow));
            Definitions.Add("sign", new FunctionDefinition("sign", Sign));
            Definitions.Add("sin", new FunctionDefinition("sin", Sin));
            Definitions.Add("cos", new FunctionDefinition("cos", Cos));
            Definitions.Add("tan", new FunctionDefinition("tan", Tan));
            Definitions.Add("asin", new FunctionDefinition("asin", Asin));
            Definitions.Add("acos", new FunctionDefinition("acos", Acos));
            Definitions.Add("atan", new FunctionDefinition("atan", Atan));
            Definitions.Add("atan2", new FunctionDefinition("atan2", Atan2));
            Definitions.Add("sinh", new FunctionDefinition("sinh", Sinh));
            Definitions.Add("cosh", new FunctionDefinition("cosh", Cosh));
            Definitions.Add("tanh", new FunctionDefinition("tanh", Tanh));
            Definitions.Add("clamp", new FunctionDefinition("clamp", Clamp));
        }

        private BuiltinFunctions()
        {
        }

        public static FunctionDefinition Find(string name)
        {
            return Definitions.TryGetValue(name, out var def) ? def : null;
        }

        private static float Round(float[] args)
        {
            EnsureArgCount("round", 1, args);
            return (float) Math.Round(args[0]);
        }

        private static float Ceil(float[] args)
        {
            EnsureArgCount("ceil", 1, args);
            return (float) Math.Ceiling(args[0]);
        }

        private static float Floor(float[] args)
        {
            EnsureArgCount("floor", 1, args);
            return (float) Math.Floor(args[0]);
        }

        private static float Abs(float[] args)
        {
            EnsureArgCount("abs", 1, args);
            return Math.Abs(args[0]);
        }

        private static float Sqrt(float[] args)
        {
            EnsureArgCount("sqrt", 1, args);
            return (float) Math.Sqrt(args[0]);
        }

        private static float Log(float[] args)
        {
            switch (args.Length)
            {
                case 1:
                    return (float) Math.Log(args[0]);
                case 2:
                    return (float) Math.Log(args[0], args[1]);
                default:
                    throw new ArgumentException($"log expects 1-2 arguments, but got {args.Length}");
            }
        }

        private static float Min(float[] args)
        {
            EnsureArgCount("min", 2, args);
            return Math.Min(args[0], args[1]);
        }

        private static float Max(float[] args)
        {
            EnsureArgCount("max", 2, args);
            return Math.Max(args[0], args[1]);
        }

        // this exists as the ^ operator too, but we add this just in case someone is looking for it
        private static float Pow(float[] args)
        {
            EnsureArgCount("pow", 2, args);
            return (float) Math.Pow(args[0], args[1]);
        }

        private static float Sign(float[] args)
        {
            EnsureArgCount("sign", 1, args);
            return Math.Sign(args[0]);
        }

        private static float Sin(float[] args)
        {
            EnsureArgCount("sin", 1, args);
            return (float) Math.Sin(args[0]);
        }

        private static float Cos(float[] args)
        {
            EnsureArgCount("cos", 1, args);
            return (float) Math.Cos(args[0]);
        }

        private static float Tan(float[] args)
        {
            EnsureArgCount("tan", 1, args);
            return (float) Math.Tan(args[0]);
        }

        private static float Asin(float[] args)
        {
            EnsureArgCount("asin", 1, args);
            return (float) Math.Asin(args[0]);
        }

        private static float Acos(float[] args)
        {
            EnsureArgCount("acos", 1, args);
            return (float) Math.Acos(args[0]);
        }

        private static float Atan(float[] args)
        {
            EnsureArgCount("atan", 1, args);
            return (float) Math.Atan(args[0]);
        }

        private static float Atan2(float[] args)
        {
            EnsureArgCount("atan2", 2, args);
            return (float) Math.Atan2(args[0], args[1]);
        }

        private static float Sinh(float[] args)
        {
            EnsureArgCount("sinh", 1, args);
            return (float) Math.Sinh(args[0]);
        }

        private static float Cosh(float[] args)
        {
            EnsureArgCount("cosh", 1, args);
            return (float) Math.Cosh(args[0]);
        }

        private static float Tanh(float[] args)
        {
            EnsureArgCount("tanh", 1, args);
            return (float) Math.Tanh(args[0]);
        }

        private static float Clamp(float[] args)
        {
            EnsureArgCount("clamp", 3, args);
            return Math.Min(Math.Max(args[0], args[1]), args[2]);
        }

        private static void EnsureArgCount(string function, int count, float[] args)
        {
            if (args.Length != count)
            {
                throw new ArgumentException($"{function} expects {count} arguments, but got {args.Length}");
            }
        }
    }
}
