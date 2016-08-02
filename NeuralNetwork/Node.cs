using System.Collections.Generic;

namespace Brain.NeuralNetwork
{
    internal class Node
    {
        public Node(IActivationFunction activation, bool initZero)
        {
            Outputs = new List<Edge>();
            Inputs = new List<Edge>();
            Activation = activation;
            if (initZero) {
                Bias = 0.0;
            }
        }

        public double Bias { get; set; }
        public IList<Edge> Inputs { get; set; }
        public IList<Edge> Outputs { get; set; }
        public double Output { get; set; }
        public double OutputDerivative { get; set; }
        public double InputDerivative { get; set; }
        public double TotalInput { get; set; }
        public double AccInputDer { get; set; }
        public int NumAccumulateDers { get; set; }
        public IActivationFunction Activation { get; set; }

        public void Update()
        {
            TotalInput = Bias;
            for (var i = 0; i < Inputs.Count; i++) {
                var e = Inputs[i];
                TotalInput += e.Weight * e.Source.Output;
            }
            Output = Activation.Compute(TotalInput);
        }
    }
}