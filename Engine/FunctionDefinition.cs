using System;

namespace NyanMath.Engine
{
    internal class FunctionDefinition
    {
        public string Name { get; private set; }
        public Func<float[], float> Delegate { get; private set; }

        public FunctionDefinition(string name, Func<float[], float> func)
        {
            Name = name;
            Delegate = func;
        }
    }
}